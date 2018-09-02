// ============================================================================
// 
// ビューアフォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using YukkoView.Shared;

namespace YukkoView
{
	// ====================================================================
	// ビューアフォーム
	// ====================================================================

	public partial class FormViewer : Form
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormViewer(FormControl oFormControl, LogWriter oLogWriter)
		{
			InitializeComponent();

			mFormControl = oFormControl;
			mLogWriter = oLogWriter;
		}

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コメント追加
		// --------------------------------------------------------------------
		public void AddComment(CommentInfo oCommentInfo)
		{
			// 連続投稿防止
			if (oCommentInfo.CompareBase(mPrevCommentInfo) && oCommentInfo.InitialTick - mPrevCommentInfo.InitialTick <= YukkoViewCommon.CONTINUOUS_PREVENT_TIME)
			{
				mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "連続投稿のため表示しません：" + oCommentInfo.Message);
				return;
			}
			mPrevCommentInfo = oCommentInfo;

			TopMostIfNeeded();

			// 描画情報設定
			SetCommentMessagePath(oCommentInfo);
			SetCommentBrush(oCommentInfo);
			CalcSpeed(oCommentInfo);

			// 位置設定（描画情報設定後に実行）
			// 文字を描画する際、X 位置ぴったりよりも少し右に描画されるので、少し左目に初期位置を設定する
			Int32 aX = CalcCommentLeft(oCommentInfo);
			Int32 aY = CalcCommentTop(oCommentInfo);
			MoveComment(oCommentInfo, aX, aY);
			mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメントを表示します。初期位置：" + aX + ", " + aY + ", 幅：" + oCommentInfo.Width
					+ ", 速度：" + oCommentInfo.Speed);

			// 追加
			lock (mCommentInfosLock)
			{
				mCommentInfos.AddLast(oCommentInfo);
			}
		}

		// --------------------------------------------------------------------
		// 表示しているコメントの数
		// --------------------------------------------------------------------
		public Int32 NumComments()
		{
			Int32 aNumComments;
			lock (mCommentInfosLock)
			{
				aNumComments = mCommentInfos.Count;
			}
			return aNumComments;
		}

		// --------------------------------------------------------------------
		// コメント表示開始
		// --------------------------------------------------------------------
		public void Start()
		{
			mIsRunning = true;
			TimerPlay.Enabled = true;
			TimerTopMost.Enabled = true;
			DrawFrame();
			UpdateControls();
		}

		// --------------------------------------------------------------------
		// コメント停止
		// --------------------------------------------------------------------
		public void Stop()
		{
			mIsRunning = false;
			TimerPlay.Enabled = false;
			TimerTopMost.Enabled = false;
			mPrevCommentInfo = null;
			DrawFrame();
			UpdateControls();
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		// 枠の太さ
		private const Int32 FRAME_WIDTH = 24;

		// 色
		private readonly Color COLOR_BG = Color.FromArgb(255, 1, 1, 1);
		private readonly Color COLOR_FRAME = Color.FromArgb(255, 0, 0, 255);

		// デフォルトフォントサイズ計算用スケール（画面縦サイズに対する比率）
		private const Single DEFAULT_FONT_SCALE = 0.07F;

		// コメントが画面端から端まで到達するのに要する時間 [ms]
		private const Int32 COMMENT_VIEWING_TIME = 12000;

		// フォント
		private const String FONT_NAME_MARLETT = "Marlett";

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// メインフォーム
		private FormControl mFormControl;

		// フォーム描画用
		private Graphics mFormGraphics;

		// オフスクリーン描画用
		private Bitmap mOffScreen;
		private Graphics mOffScreenGraphics;

		// 前回描画高さ
		private Int32 mLastDrawHeight = 0;

		// 背景ブラシ
		private SolidBrush mBgBrush;

		// 縁取りペン
		private Pen mEdgePen;

		// Init() 済
		private Boolean mInitialized = false;

		// マウス
		private Point mMouseDownPoint = Point.Empty;

		// コメント開始中
		private Boolean mIsRunning = false;

		// フォントサイズ "1" に対するピクセル数
		private Single mFontUnit;

		// コメント情報管理
		private LinkedList<CommentInfo> mCommentInfos = new LinkedList<CommentInfo>();

		// mCommentInfos ロック
		private Object mCommentInfosLock = new Object();

		// 直前のコメント
		private CommentInfo mPrevCommentInfo = null;

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コメント描画位置（水平）を算出
		// --------------------------------------------------------------------
		private Int32 CalcCommentLeft(CommentInfo oCommentInfo)
		{
			return Width - DeltaLeft(oCommentInfo) - oCommentInfo.Speed * (Environment.TickCount - oCommentInfo.InitialTick) / 1000;
		}

		// --------------------------------------------------------------------
		// 新規コメント投入時の、コメント描画位置（垂直）を算出
		// --------------------------------------------------------------------
		private Int32 CalcCommentTop(CommentInfo oNewCommentInfo)
		{
			// 新しいコメントを入れることのできる高さ範囲：Key, Value = 上端, 下端
			List<KeyValuePair<Int32, Int32>> aLayoutRange = new List<KeyValuePair<Int32, Int32>>();
			Int32 aMinTop = 0;
			Int32 aMaxBottom = Height;
			if (mFormControl.YukkoViewSettings.EnableMargin)
			{
				aMinTop = Height * mFormControl.YukkoViewSettings.MarginPercent / 100;
				aMaxBottom = Height * (100 - mFormControl.YukkoViewSettings.MarginPercent) / 100;
			}
			aLayoutRange.Add(new KeyValuePair<Int32, Int32>(aMinTop, aMaxBottom));

			// 既存コメントがある高さ範囲を除外していく
			lock (mCommentInfosLock)
			{
				foreach (CommentInfo aCommentInfo in mCommentInfos)
				{
					// ある程度中央に流れているコメントで、かつ、速度が同等以上なら逃げ切れるので範囲がかぶっても構わない
					if (aCommentInfo.Right + aCommentInfo.Height < Width - DeltaLeft(aCommentInfo) && aCommentInfo.Speed >= oNewCommentInfo.Speed)
					{
						continue;
					}

					for (Int32 i = aLayoutRange.Count - 1; i >= 0; i--)
					{
						if (aCommentInfo.Top <= aLayoutRange[i].Key && aCommentInfo.Bottom >= aLayoutRange[i].Value)
						{
							// aCommentInfo が aLayoutRange[i] を完全に覆っているので、aLayoutRange[i] を削除する
							aLayoutRange.RemoveAt(i);
						}
						else if (aCommentInfo.Top <= aLayoutRange[i].Key && (aLayoutRange[i].Key <= aCommentInfo.Bottom && aCommentInfo.Bottom < aLayoutRange[i].Value))
						{
							// aCommentInfo が aLayoutRange[i] の上方を覆っているので、aLayoutRange[i] の上端を下げる
							aLayoutRange[i] = new KeyValuePair<Int32, Int32>(aCommentInfo.Bottom + 1, aLayoutRange[i].Value);
						}
						else if ((aLayoutRange[i].Key < aCommentInfo.Top && aCommentInfo.Top <= aLayoutRange[i].Value) && aCommentInfo.Bottom >= aLayoutRange[i].Value)
						{
							// aCommentInfo が aLayoutRange[i] の下方を覆っているので、aLayoutRange[i] の下端を上げる
							aLayoutRange[i] = new KeyValuePair<Int32, Int32>(aLayoutRange[i].Key, aCommentInfo.Top - 1);
						}
						else if (aCommentInfo.Top > aLayoutRange[i].Key && aCommentInfo.Bottom < aLayoutRange[i].Value)
						{
							// aCommentInfo が aLayoutRange[i] の内側にあるので、aLayoutRange[i] を分割する
							KeyValuePair<Int32, Int32> aRange = aLayoutRange[i];
							aLayoutRange[i] = new KeyValuePair<Int32, Int32>(aRange.Key, aCommentInfo.Top - 1);
							aLayoutRange.Add(new KeyValuePair<Int32, Int32>(aCommentInfo.Bottom + 1, aRange.Value));
						}
						else
						{
							// aCommentInfo は覆っていない
						}
					}
				}
			}

			// 新しいコメントが入る範囲があるなら位置決め
			foreach (KeyValuePair<Int32, Int32> aRange in aLayoutRange)
			{
				if (aRange.Value - aRange.Key + 1 >= oNewCommentInfo.Height)
				{
					return aRange.Key;
				}
			}

			// 新しいコメントが入る範囲がないので弾幕モードとする
			Random aRand = new Random();
			return aRand.Next(aMaxBottom - aMinTop - oNewCommentInfo.Height) + aMinTop;
		}

		// --------------------------------------------------------------------
		// コメント移動速度
		// --------------------------------------------------------------------
		private void CalcSpeed(CommentInfo oCommentInfo)
		{
			oCommentInfo.Speed = (ClientSize.Width + oCommentInfo.Width) / (COMMENT_VIEWING_TIME / 1000);
		}

		// --------------------------------------------------------------------
		// 新規コメント投入時の、画面端からの食い込み量
		// --------------------------------------------------------------------
		private Int32 DeltaLeft(CommentInfo oCommentInfo)
		{
			return oCommentInfo.Height / 2;
		}

		// --------------------------------------------------------------------
		// サイズ知覚用の枠を描画
		// --------------------------------------------------------------------
		private void DrawFrame()
		{
			if (mIsRunning)
			{
				// 枠無し（全透明）
				mFormGraphics.FillRectangle(mBgBrush, 0, 0, ClientSize.Width, ClientSize.Height);
			}
			else
			{
				// 枠あり
				using (SolidBrush aBrush = new SolidBrush(COLOR_FRAME))
				{
					mFormGraphics.FillRectangle(aBrush, 0, 0, ClientSize.Width, ClientSize.Height);
				}
				mFormGraphics.FillRectangle(mBgBrush, FRAME_WIDTH, FRAME_WIDTH, ClientSize.Width - FRAME_WIDTH * 2, ClientSize.Height - FRAME_WIDTH * 2);
			}
		}

		// --------------------------------------------------------------------
		// 初期化
		// FormBorderStyle が None の場合、Load() イベントよりも Resize() イベントが
		// 先に発生するようなので、どちらが先に発生しても大丈夫なように、
		// mInitialized フラグで管理する
		// --------------------------------------------------------------------
		private void Init()
		{
			if (mInitialized)
			{
				return;
			}

			// コントロール
			ButtonDisplay.Font = new Font(FONT_NAME_MARLETT, Font.Size);
			ButtonMaximize.Font = new Font(FONT_NAME_MARLETT, Font.Size);
			LabelGrip.Font = new Font(FONT_NAME_MARLETT, Font.Size);

			// フォーム描画用
			mFormGraphics = Graphics.FromHwnd(Handle);

			// オフスクリーン描画用
			mOffScreen = new Bitmap(1, 1);
			mOffScreenGraphics = Graphics.FromImage(mOffScreen);

			// ブラシ
			mBgBrush = new SolidBrush(COLOR_BG);

			// ペン
			mEdgePen = new Pen(Color.Black, YukkoViewCommon.EDGE_WIDTH);

			// 背景色
			BackColor = COLOR_BG;
			TransparencyKey = COLOR_BG;

			mInitialized = true;
		}

		// --------------------------------------------------------------------
		// コメント移動
		// --------------------------------------------------------------------
		private void MoveComment(CommentInfo oCommentInfo, Int32 oDx, Int32 oDy)
		{
			Matrix aTranslateMatrix = new Matrix();
			aTranslateMatrix.Translate(oDx, oDy);
			oCommentInfo.MessagePath.Transform(aTranslateMatrix);
			oCommentInfo.SpecifyLeft += oDx;
		}

		// --------------------------------------------------------------------
		// リサイズ処理
		// --------------------------------------------------------------------
		private void ResizeResources()
		{
			// 破棄
			mFormGraphics.Dispose();
			mOffScreenGraphics.Dispose();
			mOffScreen.Dispose();

			// 再生成
			mFormGraphics = Graphics.FromHwnd(Handle);
			mOffScreen = new Bitmap(Width, Height);
			mOffScreenGraphics = Graphics.FromImage(mOffScreen);

			// フォントサイズ
			mFontUnit = ClientSize.Height * DEFAULT_FONT_SCALE / YukkoViewCommon.DEFAULT_YUKARI_FONT_SIZE;
		}

		// --------------------------------------------------------------------
		// コメントのブラシを設定
		// --------------------------------------------------------------------
		private void SetCommentBrush(CommentInfo oCommentInfo)
		{
			oCommentInfo.Brush = new SolidBrush(oCommentInfo.Color);
		}

		// --------------------------------------------------------------------
		// コメントのグラフィックスパスを設定
		// --------------------------------------------------------------------
		private void SetCommentMessagePath(CommentInfo oCommentInfo)
		{
			// フォントのサイズ補正
			Int32 aSize = oCommentInfo.YukariSize;
			if (aSize < 1)
			{
				aSize = 1;
			}

			oCommentInfo.MessagePath = new GraphicsPath();
			oCommentInfo.MessagePath.AddString(oCommentInfo.Message, Font.FontFamily, 0, aSize * mFontUnit, Point.Empty, StringFormat.GenericDefault);
		}

		// --------------------------------------------------------------------
		// ウィンドウを最前面に出す（必要に応じて）
		// --------------------------------------------------------------------
		private void TopMostIfNeeded()
		{
			if (!Visible)
			{
				return;
			}

			lock (mCommentInfosLock)
			{
				if (mCommentInfos.Count == 0)
				{
					return;
				}
			}

			// MPC-BE のフルスクリーンは通常の最大化とは異なり、このフォームの TopMost が効かない
			// MPC-BE が最前面に来ている時は、このフォームを最前面に持って行く必要がある
			// GetForegroundWindow() はマルチディスプレイに関係なく最前面を報告するので、
			// ディスプレイ 2 の最前面が MPC-BE でも、ディスプレイ 1 に他のアプリがあってそれが最前面ならそれを報告する
			// 従って、MPC-BE が当該ディスプレイで最前面かどうか判定不能
			// そこで、このフォームが最前面でない場合は常に最前面に出すことにする
			// タスクスイッチなどシステム系が最前面になっている場合にこのフォームを最前面にしても、今のところ問題ない模様
			IntPtr aFGHandle = WindowsApi.GetForegroundWindow();
			if (aFGHandle != Handle)
			{
				WindowsApi.BringWindowToTop(Handle);
			}
		}

		// --------------------------------------------------------------------
		// ボタン・ラベルの状態を更新
		// --------------------------------------------------------------------
		private void UpdateControls()
		{
			// ディスプレイ切り替え
			ButtonDisplay.Visible = !mIsRunning;
			ButtonDisplay.Enabled = Screen.AllScreens.Length > 1;

			// 最大化
			ButtonMaximize.Visible = !mIsRunning;
			if (WindowState == FormWindowState.Maximized)
			{
				ButtonMaximize.Text = "2";
				ToolTipViewer.SetToolTip(ButtonMaximize, "元に戻す");
			}
			else
			{
				ButtonMaximize.Text = "1";
				ToolTipViewer.SetToolTip(ButtonMaximize, "最大化");
			}

			// リサイズグリップ
			LabelGrip.Visible = !mIsRunning && WindowState != FormWindowState.Maximized;
		}

		// ====================================================================
		// IDE 生成イベントハンドラー
		// ====================================================================

		private void FormViewer_Resize(object sender, EventArgs e)
		{
			try
			{
				// Load() よりも Resize() が先に呼ばれる場合の対策
				Init();

				// リサイズ時の処理
				ResizeResources();
				DrawFrame();
				UpdateControls();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "リサイズ時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_Load(object sender, EventArgs e)
		{
			try
			{
				Init();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアーロード時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				// キャンバス破棄
				mFormGraphics.Dispose();
				mBgBrush.Dispose();
				mEdgePen.Dispose();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアクローズ時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_Shown(object sender, EventArgs e)
		{
			try
			{
				ResizeResources();
				UpdateControls();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアー表示時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				DrawFrame();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアー再描画時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "FormViewer_MouseDown() name: " + ((Control)sender).Name);
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left && WindowState != FormWindowState.Maximized)
				{
					mMouseDownPoint = new Point(e.X, e.Y);
					((Control)sender).Capture = true;
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアーマウスダウン時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (Capture && WindowState != FormWindowState.Maximized)
				{
					// ウィンドウ移動
					Left += e.X - mMouseDownPoint.X;
					Top += e.Y - mMouseDownPoint.Y;
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアーマウス移動時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void TimerPlay_Tick(object sender, EventArgs e)
		{
			try
			{
				Int32 aThisDrawHeight = 0;

				lock (mCommentInfosLock)
				{
					// オフスクリーンの描画高さ設定
					foreach (CommentInfo aCommentInfo in mCommentInfos)
					{
						if (aCommentInfo.Bottom > aThisDrawHeight)
						{
							aThisDrawHeight = aCommentInfo.Bottom;
						}
					}
					Int32 aMargeDrawHeight = Math.Max(mLastDrawHeight, aThisDrawHeight);
					if (aMargeDrawHeight == 0)
					{
						return;
					}

					// クリア
					mOffScreenGraphics.FillRectangle(mBgBrush, 0, 0, Width, aMargeDrawHeight);

					// コメント描画と移動
					LinkedListNode<CommentInfo> aNode = mCommentInfos.First;
					while (aNode != null)
					{
						CommentInfo aCommentInfo = aNode.Value;
						mOffScreenGraphics.DrawPath(mEdgePen, aCommentInfo.MessagePath);
						mOffScreenGraphics.FillPath(aCommentInfo.Brush, aCommentInfo.MessagePath);
						MoveComment(aCommentInfo, CalcCommentLeft(aCommentInfo) - aCommentInfo.SpecifyLeft, 0);

						if (aCommentInfo.Right <= 0)
						{
							LinkedListNode<CommentInfo> aRemovingNode = aNode;
							aNode = aNode.Next;

							// 移動完了したのでコメントオブジェクトを解放
							mLogWriter.ShowLogMessage(TraceEventType.Verbose, "TimerPlay_Tick() remove: " + aCommentInfo.Message);
							aCommentInfo.Dispose();
							mCommentInfos.Remove(aRemovingNode);
						}
						else
						{
							aNode = aNode.Next;
						}
					}

					// 描画
					mFormGraphics.DrawImage(mOffScreen, 0, 0, new Rectangle(0, 0, Width, aMargeDrawHeight), GraphicsUnit.Pixel);

				}

				// 後処理
				mLastDrawHeight = aThisDrawHeight;
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "タイマー発動時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormViewer_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				if (((Control)sender).Capture)
				{
					((Control)sender).Capture = false;
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ビューアーマウスアップ時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void LabelGrip_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (LabelGrip.Capture && WindowState != FormWindowState.Maximized)
				{
					// リサイズ
					Width += e.X - mMouseDownPoint.X;
					Height += e.Y - mMouseDownPoint.Y;
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "グリップマウス移動時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}

		}

		private void ButtonMaximize_Click(object sender, EventArgs e)
		{
			try
			{
				if (WindowState == FormWindowState.Maximized)
				{
					WindowState = FormWindowState.Normal;
				}
				else
				{
					WindowState = FormWindowState.Maximized;
				}

				// メインウィンドウにフィードバック
				mFormControl.SetComboBoxWindowState(WindowState);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "最大化時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonDisplay_Click(object sender, EventArgs e)
		{
			try
			{
				// 現在のディスプレイを把握
				Int32 aCurrentScreen = -1;
				for (Int32 i = 0; i < Screen.AllScreens.Length; i++)
				{
					if (Screen.AllScreens[i].Bounds.IntersectsWith(Bounds))
					{
						aCurrentScreen = i;
						break;
					}
				}

				// フルスクリーンのままだと移動しないので一旦解除
				FormWindowState aWindowStateBak = WindowState;
				WindowState = FormWindowState.Normal;

				// 次のディスプレイに移動
				aCurrentScreen++;
				if (aCurrentScreen >= Screen.AllScreens.Length)
				{
					aCurrentScreen = 0;
				}
				Location = Screen.AllScreens[aCurrentScreen].Bounds.Location;
				WindowState = aWindowStateBak;

				// メインウィンドウにフィードバック
				mFormControl.SetComboBoxDisplay(aCurrentScreen);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ディスプレイ切り替え時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void TimerTopMost_Tick(object sender, EventArgs e)
		{
			TopMostIfNeeded();
		}
	} // public partial class FormViewer

} // namespace YukkoView
