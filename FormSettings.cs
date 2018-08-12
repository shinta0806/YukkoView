// ============================================================================
// 
// 環境設定フォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using YukkoView.Shared;

namespace YukkoView
{
	// ====================================================================
	// ビューアフォーム
	// ====================================================================

	public partial class FormSettings : Form
	{
		// ====================================================================
		// public プロパティー
		// ====================================================================

		// 設定
		public YukkoViewSettings YukkoViewSettings { get; set; }

		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormSettings(LogWriter oLogWriter)
		{
			InitializeComponent();

			mLogWriter = oLogWriter;
		}


		// ====================================================================
		// protected メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// メッセージハンドラ
		// --------------------------------------------------------------------
		protected override void WndProc(ref Message oMsg)
		{
			if (oMsg.Msg == UpdaterLauncher.WM_UPDATER_UI_DISPLAYED)
			{
				WMUpdaterUIDisplayed();
			}
			base.WndProc(ref oMsg);
		}

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 入力チェック
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void CheckInput()
		{
			// 上下マージン
			Int32 aMarginPercent;
			if (!Int32.TryParse(TextBoxMarginPercent.Text, out aMarginPercent))
			{
				throw new Exception("ビューアウィンドウの上下 [%] には数値を入力して下さい。");
			}
			if (aMarginPercent < YukkoViewCommon.MARGIN_PERCENT_MIN || aMarginPercent > YukkoViewCommon.MARGIN_PERCENT_MAX)
			{
				throw new Exception("ビューアウィンドウの上下は " + YukkoViewCommon.MARGIN_PERCENT_MIN.ToString() + " [%] ～"
						+ YukkoViewCommon.MARGIN_PERCENT_MAX.ToString() + " [%] の間で入力して下さい。");
			}
		}

		// --------------------------------------------------------------------
		// コンポーネントから設定を取得
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void CompoToSettings()
		{
			CheckInput();

			// 格納
			YukkoViewSettings.AutoRun = CheckBoxAutoRun.Checked;
			YukkoViewSettings.EnableMargin = CheckBoxEnableMargin.Checked;
			YukkoViewSettings.MarginPercent = Int32.Parse(TextBoxMarginPercent.Text);
			YukkoViewSettings.CheckRss = CheckBoxCheckRss.Checked;
		}

		// --------------------------------------------------------------------
		// 進捗系のコンポーネントをすべて元に戻す
		// --------------------------------------------------------------------
		private void MakeAllComposNormal()
		{
			ProgressBarCheckRss.Visible = false;
			ButtonCheckRss.Enabled = true;
		}

		// --------------------------------------------------------------------
		// 最新情報確認コンポーネントを進捗中にする
		// --------------------------------------------------------------------
		private void MakeLatestComposRunning()
		{
			ProgressBarCheckRss.Visible = true;

			// ボタンは全部無効化
			ButtonCheckRss.Enabled = false;
		}

		// --------------------------------------------------------------------
		// 設定をコンポーネントに反映
		// --------------------------------------------------------------------
		private void SettingsToCompo()
		{
			// 起動コメント開始
			CheckBoxAutoRun.Checked = YukkoViewSettings.AutoRun;

			// 上下マージン
			CheckBoxEnableMargin.Checked = YukkoViewSettings.EnableMargin;
			TextBoxMarginPercent.Text = YukkoViewSettings.MarginPercent.ToString();
			UpdateTextBoxMarginPercent();

			// RSS
			CheckBoxCheckRss.Checked = YukkoViewSettings.CheckRss;
		}

		// --------------------------------------------------------------------
		// マージンテキストボックスの状態を更新
		// --------------------------------------------------------------------
		private void UpdateTextBoxMarginPercent()
		{
			// YukkoViewSettings はリアルタイムで更新されないので、CheckBoxEnableMargin の値で判定する
			TextBoxMarginPercent.Enabled = CheckBoxEnableMargin.Checked;
		}

		// --------------------------------------------------------------------
		// ちょちょいと自動更新の画面が何かしら表示された
		// --------------------------------------------------------------------
		private void WMUpdaterUIDisplayed()
		{
			MakeAllComposNormal();
		}

		// ====================================================================
		// IDE 生成イベントハンドラー
		// ====================================================================

		private void FormSettings_Load(object sender, EventArgs e)
		{
			try
			{
				Text = YukkoViewCommon.APP_NAME_J + "の環境設定";
#if DEBUG
				Text = "［デバッグ］" + Text;
#endif

				Common.CascadeForm(this);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "環境設定ロード時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormSettings_Shown(object sender, EventArgs e)
		{
			try
			{
				SettingsToCompo();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "環境設定表示時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void CheckBoxEnableMargin_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateTextBoxMarginPercent();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "マージン有効無効切替時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void CheckBoxCheckRss_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckBoxCheckRss.Checked)
				{
					return;
				}
				if (MessageBox.Show("最新情報・更新版の確認を無効にすると、" + YukkoViewCommon.APP_NAME_J
						+ "の新版がリリースされても自動的にインストールされず、古いバージョンを使い続けることになります。\n"
						+ "本当に無効にしてもよろしいですか？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
						!= DialogResult.Yes)
				{
					CheckBoxCheckRss.Checked = true;
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "更新有効無効切替時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			try
			{
				CompoToSettings();

				// RSS チェックが無効になっていた場合は RSS 状態ファイルを削除
				// 再度有効にされた時に、たまってた更新情報がどばっと表示されるのを防止するため
				if (!YukkoViewSettings.CheckRss)
				{
					try
					{
						File.Delete(YukkoViewCommon.RssIniPath());
					}
					catch
					{
					}
				}

				DialogResult = DialogResult.OK;
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "環境設定保存時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonCheckRss_Click(object sender, EventArgs e)
		{
			try
			{
				MakeLatestComposRunning();
				if (!YukkoViewCommon.LaunchUpdater(true, true, Handle, true, false, mLogWriter))
				{
					MakeAllComposNormal();
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "最新情報確認時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}

		}

		private void ButtonLog_Click(object sender, EventArgs e)
		{
			try
			{
				SaveFileDialogLog.FileName = "YukkoViewLog_" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss");
				if (SaveFileDialogLog.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				// 環境情報保存
				YukkoViewCommon.LogEnvironmentInfo(mLogWriter);

				ZipFile.CreateFromDirectory(YukkoViewCommon.SettingsPath(), SaveFileDialogLog.FileName, CompressionLevel.Optimal, true);
				mLogWriter.ShowLogMessage(TraceEventType.Information, "ログ保存完了：\n" + SaveFileDialogLog.FileName);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ログ保存時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}

		}
	}
}
