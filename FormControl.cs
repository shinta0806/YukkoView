﻿// ============================================================================
// 
// メインフォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using YukkoView.Shared;

namespace YukkoView
{
	// ====================================================================
	// メインフォーム
	// ====================================================================

	public partial class FormControl : Form
	{
		// ====================================================================
		// public プロパティー
		// ====================================================================

		// 設定
		public YukkoViewSettings YukkoViewSettings
		{
			get
			{
				return mYukkoViewSettings;
			}
		}

		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormControl()
		{
			InitializeComponent();
		}

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// ディスプレイの選択肢を最新化した後、指定された値にセット
		// ＜サブスレッドから呼びだし可能なコントロール操作関数＞
		// --------------------------------------------------------------------
		public void SetComboBoxDisplay(Int32 oIndex)
		{
			Invoke(new Action(() =>
			{
				// 選択肢の最新化
				UpdateComboBoxDisplay();

				// 選択肢の調整
				if (oIndex < 0 || oIndex >= ComboBoxDisplay.Items.Count)
				{
					oIndex = 0;
				}

				ComboBoxDisplay.SelectedIndex = oIndex;
			}));
		}

		// --------------------------------------------------------------------
		// ウィンドウの状態コンボボックスを指定された値にセット
		// ＜サブスレッドから呼びだし可能なコントロール操作関数＞
		// --------------------------------------------------------------------
		public void SetComboBoxWindowState(FormWindowState oCommentWindowState)
		{
			Invoke(new Action(() =>
			{
				if (oCommentWindowState == FormWindowState.Maximized)
				{
					ComboBoxWindowState.SelectedIndex = 0;
				}
				else
				{
					ComboBoxWindowState.SelectedIndex = 1;
				}
			}));
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		// 改訂履歴
		private const String FILE_NAME_HISTORY = "YukkoView_History_JPN.txt";

		// 色
		private readonly Color COLOR_STATUS_ERROR = Color.Red;
		private readonly Color COLOR_STATUS_NOTICE = Color.Blue;
		private readonly Color COLOR_STATUS_RUNNING = Color.LimeGreen;
		private readonly Color COLOR_STATUS_STOP = Color.Black;

		// 古すぎて無視するコメントの閾値 [時間]
		private const Int32 IGNORE_HOUR = 12;

		// テストコメント投稿用の色
		private readonly Color[] COLOR_TEST_COMMENTS = { Color.White, Color.Gray, Color.Pink, Color.Red, Color.Orange, Color.Yellow,
				Color.Lime, Color.Cyan, Color.Blue, Color.Purple, Color.FromArgb(255, 0x11, 0x11, 0x11) };

		// TCP タイムアウト [ms]
		private const Int32 TCP_TIMEOUT = 5 * 1000;

		// コメント識別子
		private const String COMMENT_BEGIN_MARK = "Comment=";

		// ログ用
		private const String TRACE_SOURCE_NAME = "Ycv";

		// ゆかり設定取得用
		private const String YUKARI_CONFIG_KEY_NAME_SERVER_URL = "commenturl_base";
		private const String YUKARI_CONFIG_KEY_NAME_ROOM_NAME = "commentroom";

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// ビューアウィンドウ
		private FormViewer mFormViewer;

		// ゆかり設定読み込み状況
		private Boolean mIsYukariConfigError = false;

		// ゆかり設定読み込みエラー内容
		private String mYukariConfigErrorMessage;

		// コメント開始中
		private Boolean mIsRunning = false;

		// コメント受信状況
		private Boolean mIsCommentReceiveError = false;

		// テストコメントの色番号
		private Int32 mTestCommentColorIndex = 0;

		// 設定
		private YukkoViewSettings mYukkoViewSettings = new YukkoViewSettings();

		// 停止ボタン判定用
		private CancellationTokenSource mStopCancellationTokenSource = new CancellationTokenSource();

		// 終了時タスク安全中断用
		private CancellationTokenSource mClosingCancellationTokenSource = new CancellationTokenSource();

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// バイト列からを解析
		// --------------------------------------------------------------------
		private CommentInfo AnalyzeCommentData(Byte[] oArray)
		{
			// 先頭の改行を無視する
			Int32 aBeginPos = 0;
			while (aBeginPos < oArray.Length && (oArray[aBeginPos] == '\r' || oArray[aBeginPos] == '\n'))
			{
				aBeginPos++;
			}
			if (aBeginPos == oArray.Length)
			{
				// コメントの中身が無い
				return null;
			}

			// 文字列を解析してコメント情報化
			CommentInfo aCommentInfo;
			if (oArray[aBeginPos] == 'X')
			{
				aCommentInfo = AnalyzeExtendedCommentData(oArray, aBeginPos);
			}
			else
			{
				aCommentInfo = AnalyzeOldFormatCommentData(oArray, aBeginPos);
			}
			return aCommentInfo;
		}

		// --------------------------------------------------------------------
		// 拡張コメント文字列を解析
		// --------------------------------------------------------------------
		private CommentInfo AnalyzeExtendedCommentData(Byte[] oArray, Int32 oBeginPos)
		{
			return AnalyzeExtendedCommentData(Encoding.UTF8.GetString(oArray, oBeginPos, oArray.Length - oBeginPos));
		}

		// --------------------------------------------------------------------
		// 拡張コメント文字列を解析
		// --------------------------------------------------------------------
		private CommentInfo AnalyzeExtendedCommentData(String oComment)
		{
			// 拡張バージョン識別子の確認
			if (oComment.Substring(1, 1) != "3")
			{
				throw new Exception("未対応の拡張コメントフォーマットです。");
			}

			// 古いコメントは無視
			DateTime aCommentTime = DateTime.ParseExact(oComment.Substring(9, 19), "yyyy-MM-dd HH:mm:ss", null);
			String aCommentMessage = oComment.Substring(28, oComment.Length - 29);
			if (DateTime.Now.Subtract(aCommentTime) >= new TimeSpan(IGNORE_HOUR, 0, 0))
			{
				mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, IGNORE_HOUR + "時間以上経過しているコメントを無視します：" + aCommentMessage);
				return null;
			}

			CommentInfo aCommentInfo = new CommentInfo();
			aCommentInfo.Message = aCommentMessage;
			aCommentInfo.YukariSize = Int32.Parse(oComment.Substring(2, 1));
			aCommentInfo.Color = Color.FromArgb((Int32)(Convert.ToInt32(oComment.Substring(3, 6), 16) | 0xFF000000));
			aCommentInfo.InitialTick = Environment.TickCount;
			return aCommentInfo;
		}

		// --------------------------------------------------------------------
		// 旧仕様コメント文字列を解析
		// --------------------------------------------------------------------
		private CommentInfo AnalyzeOldFormatCommentData(Byte[] oArray, Int32 oBeginPos)
		{
			String aComment = Encoding.GetEncoding(Common.CODE_PAGE_SHIFT_JIS).GetString(oArray, oBeginPos, oArray.Length - oBeginPos);

			if (aComment == "nothing" || aComment.Length <= 7)
			{
				return null;
			}

			CommentInfo aCommentInfo = new CommentInfo();
			aCommentInfo.Message = aComment.Substring(7, aComment.Length - 8);
			aCommentInfo.YukariSize = Int32.Parse(aComment.Substring(0, 1));
			aCommentInfo.Color = Color.FromArgb((Int32)(Convert.ToInt32(aComment.Substring(1, 6), 16) | 0xFF000000));
			aCommentInfo.InitialTick = Environment.TickCount;
			return aCommentInfo;
		}

		// --------------------------------------------------------------------
		// mIsCommentReceiveError をクリア
		// --------------------------------------------------------------------
		private void ClearIsCommentReceiveError()
		{
			if (mIsCommentReceiveError)
			{
				mIsCommentReceiveError = false;
				UpdateStatusLabels();
			}
		}

		// --------------------------------------------------------------------
		// コメントサーバーからコメントをダウンロード
		// --------------------------------------------------------------------
		private Byte[] DownloadComment(Downloader oDownloader)
		{
			Byte[] aArray;
			using (MemoryStream aMemStream = new MemoryStream())
			{
				oDownloader.Download(mYukkoViewSettings.ServerUrl + "?r=" + HttpUtility.UrlEncode(mYukkoViewSettings.RoomName, Encoding.UTF8) + "&v=3",
						aMemStream);
				aArray = aMemStream.ToArray();
			}
			if (aArray.Length == 0)
			{
				throw new Exception("コメントサーバーのデータが空です。");
			}

			return aArray;
		}

		// --------------------------------------------------------------------
		// コメントサーバーからコメントをダウンロード
		// --------------------------------------------------------------------
		private Task DownloadCommentAsync()
		{
			return Task.Run(() =>
			{
				try
				{
					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメントダウンロード開始");
					CancellationTokenSource aStopCancellationTokenSourceBak = mStopCancellationTokenSource;

					// ダウンロード
					Downloader aDownloader = new Downloader();
					aDownloader.CancellationToken = aStopCancellationTokenSourceBak.Token;

					for (; ; )
					{
						try
						{
							// サーバーに溜まっているコメントをすべて読み込む
							for (; ; )
							{
								// コメント表示数が多い場合はスキップ
								if (mFormViewer.NumComments() >= YukkoViewCommon.NUM_DISPLAY_COMMENTS_MAX)
								{
									break;
								}

								// サーバーの負荷軽減のためちょっとだけ休む
								Thread.Sleep(Common.GENERAL_SLEEP_TIME);

								// ダウンロード
								Byte[] aArray = DownloadComment(aDownloader);

								// サーバーとの通信に成功したのでエラー表示解除
								ClearIsCommentReceiveError();

								// コメント
								CommentInfo aCommentInfo = AnalyzeCommentData(aArray);
								if (aCommentInfo == null)
								{
									// 溜まっているコメントが無くなった
									break;
								}
								mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメントをダウンロードしました：" + aCommentInfo.Message);

								// コメント発行
								mFormViewer.AddComment(aCommentInfo);
							}

						}
						catch (Exception oExcep)
						{
							mLogWriter.LogMessage(TraceEventType.Error, "ダウンロードエラー（リトライします）：\n" + oExcep.Message);
							mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
							EnableIsCommentReceiveError();
						}

						// しばらく休憩
						Thread.Sleep(mYukkoViewSettings.Interval);
						aStopCancellationTokenSourceBak.Token.ThrowIfCancellationRequested();
						mClosingCancellationTokenSource.Token.ThrowIfCancellationRequested();
					}

				}
				catch (OperationCanceledException)
				{
					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "ダウンロード処理を終了しました。");
				}
				catch (Exception oExcep)
				{
					mLogWriter.ShowLogMessage(TraceEventType.Error, "ダウンロードエラー：\n" + oExcep.Message);
					mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
				}
			});
		}

		// --------------------------------------------------------------------
		// mIsCommentReceiveError を立てる
		// --------------------------------------------------------------------
		private void EnableIsCommentReceiveError()
		{
			if (!mIsCommentReceiveError)
			{
				mIsCommentReceiveError = true;
				UpdateStatusLabels();
			}
		}

		// --------------------------------------------------------------------
		// ゆかり設定を取得
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void GetYukariConfig(String[] oLines, String oKeyName, out String oValue)
		{
			// キーを検索
			Int32 aIndex = -1;
			for (Int32 i = 0; i < oLines.Length; i++)
			{
				if (oLines[i].IndexOf(oKeyName + "=") == 0)
				{
					aIndex = i;
					break;
				}
			}
			if (aIndex < 0)
			{
				throw new Exception("設定項目 " + oKeyName + " が見つかりません。");
			}

			// 値を取得
			oValue = HttpUtility.UrlDecode(oLines[aIndex].Substring(oKeyName.Length + 1));
		}

		// --------------------------------------------------------------------
		// 初期化
		// --------------------------------------------------------------------
		private void Init()
		{
			// ログ初期化
			mLogWriter = new LogWriter(TRACE_SOURCE_NAME);
			mLogWriter.ApplicationQuitToken = mClosingCancellationTokenSource.Token;
			YukkoViewCommon.LogWriter = mLogWriter;
			mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "起動しました：" + YukkoViewCommon.APP_NAME_J + " "
					+ YukkoViewCommon.APP_VER + " ====================");
#if DEBUG
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "デバッグモード：" + Common.DEBUG_ENABLED_MARK);
#endif

			// カレントフォルダー正規化（ゆかりから起動された場合はゆかりのフォルダーになっているため）
			Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);

			// タイトルバー
			Text = YukkoViewCommon.APP_NAME_J;
#if DEBUG
			Text = "［デバッグ］" + Text;
#endif

			// 設定の読み込み
			mYukkoViewSettings.Reload();

			// ビューアウィンドウ
			mFormViewer = new FormViewer(this, mLogWriter);
		}

		// --------------------------------------------------------------------
		// ゆかり設定を読み込む
		// --------------------------------------------------------------------
		private void LoadYukariConfig()
		{
			try
			{
				mIsYukariConfigError = false;

				// ファイル読み込み
				String[] aLines = File.ReadAllLines(mYukkoViewSettings.YukariConfigPath);

				// 設定取得
				String aString;
				GetYukariConfig(aLines, YUKARI_CONFIG_KEY_NAME_SERVER_URL, out aString);
				if (String.IsNullOrEmpty(aString) || aString.LastIndexOf("/") < 0)
				{
					throw new Exception("コメントサーバーが設定されていません。");
				}
				mYukkoViewSettings.ServerUrl = aString.Substring(0, aString.LastIndexOf("/")) + "/c.php";
				GetYukariConfig(aLines, YUKARI_CONFIG_KEY_NAME_ROOM_NAME, out aString);
				mYukkoViewSettings.RoomName = aString;
			}
			catch (Exception oExcep)
			{
				mIsYukariConfigError = true;
				mYukariConfigErrorMessage = oExcep.Message;
				mLogWriter.LogMessage(TraceEventType.Error, "ゆかり設定読み込みエラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		// --------------------------------------------------------------------
		// ビューアウィンドウを移動
		// --------------------------------------------------------------------
		private void MoveFormViewer()
		{
			// 移動中はフォームを非表示にしようと思ったが、そうすると、最大化した後通常に戻らなくなるのでやめた
			mFormViewer.WindowState = FormWindowState.Normal;
			Screen[] aScreens = Screen.AllScreens;
			Int32 aScreenIndex = ComboBoxDisplay.SelectedIndex;
			if (aScreenIndex >= aScreens.Length)
			{
				aScreenIndex = aScreens.Length - 1;
			}
			mFormViewer.Location = aScreens[aScreenIndex].Bounds.Location;
			mFormViewer.WindowState = mYukkoViewSettings.CommentWindowState;
		}

		// --------------------------------------------------------------------
		// 新バージョンで初回起動された時の処理を行う
		// --------------------------------------------------------------------
		private void NewVersionLaunched()
		{
			String aNewVerMsg;

			// α・β警告、ならびに、更新時のメッセージ（2017/01/09）
			// 新規・更新のご挨拶
			if (String.IsNullOrEmpty(mYukkoViewSettings.PrevLaunchVer))
			{
				// 新規
				aNewVerMsg = "【初回起動】\n\n";
				aNewVerMsg += YukkoViewCommon.APP_NAME_J + "をダウンロードしていただき、ありがとうございます。";
			}
			else
			{
				aNewVerMsg = "【更新起動】\n\n";
				aNewVerMsg += YukkoViewCommon.APP_NAME_J + "を更新していただき、ありがとうございます。\n";
				aNewVerMsg += "更新内容については［ヘルプ→改訂履歴］メニューをご参照ください。";
			}

			// α・βの注意
			if (YukkoViewCommon.APP_VER.IndexOf("α") >= 0)
			{
				aNewVerMsg += "\n\nこのバージョンは開発途上のアルファバージョンです。\n"
						+ "使用前にヘルプをよく読み、注意してお使い下さい。";
			}
			else if (YukkoViewCommon.APP_VER.IndexOf("β") >= 0)
			{
				aNewVerMsg += "\n\nこのバージョンは開発途上のベータバージョンです。\n"
						+ "使用前にヘルプをよく読み、注意してお使い下さい。";
			}

			// 表示
			mLogWriter.ShowLogMessage(TraceEventType.Information, aNewVerMsg);

			// Zone ID 削除
			Common.DeleteZoneID(Path.GetDirectoryName(Application.ExecutablePath), SearchOption.AllDirectories);
		}

		// --------------------------------------------------------------------
		// ゆかり設定ラジオボタンの選択が変更された場合のイベントハンドラー
		// --------------------------------------------------------------------
		private void RadioButtonConfig_CheckedChanged(Object oSender, EventArgs oEventArgs)
		{
			try
			{
				// 設定変更
				if (RadioButtonYukari.Checked)
				{
					mYukkoViewSettings.ServerSettingsType = ServerSettingsType.Auto;
					LoadYukariConfig();
					SettingsToCompos();
				}
				else
				{
					mYukkoViewSettings.ServerSettingsType = ServerSettingsType.Manual;

					// ラジオボタンが変更された際は ServerUrl と RoomName に変更はないので再取得しない
					{
					}
				}
				mYukkoViewSettings.Save();

				UpdateServerComposEnabled();
				SetFileSystemWatcherYukariConfig();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ゆかり設定指定方法変更時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		// --------------------------------------------------------------------
		// コメントサーバーからプッシュ通知を受信
		// --------------------------------------------------------------------
		private Task ReceivePushCommentAsync()
		{
			return Task.Run(() =>
			{
				TcpListener aListener = null;
				try
				{
					CancellationTokenSource aStopCancellationTokenSourceBak = mStopCancellationTokenSource;

					// ゆかり通信チェック
					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "ゆかり通信チェック");
					Downloader aDownloader = new Downloader();
					aDownloader.CancellationToken = aStopCancellationTokenSourceBak.Token;
					for (; ; )
					{
						try
						{
							Byte[] aArray = DownloadComment(aDownloader);

							// エラー無くダウンロードできたらチェックループ終了
							ClearIsCommentReceiveError();
							break;
						}
						catch (Exception)
						{
						}

						// エラー有りの場合はループを続ける
						EnableIsCommentReceiveError();
						Thread.Sleep(YukkoViewCommon.CHECK_CONNECTION_INTERVAL);

						aStopCancellationTokenSourceBak.Token.ThrowIfCancellationRequested();
						mClosingCancellationTokenSource.Token.ThrowIfCancellationRequested();
					}

					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメントプッシュ受信開始");

					// IPv4 と IPv6 の全ての IP アドレスを Listen する
					aListener = new TcpListener(IPAddress.IPv6Any, mYukkoViewSettings.ReceivePushPort);
					aListener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
					aListener.Start();
					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "IP アドレス：" + ((IPEndPoint)aListener.LocalEndpoint).Address
							+ ", ポート：" + ((IPEndPoint)aListener.LocalEndpoint).Port);

					for (; ; )
					{
						try
						{
							// 接続要求があったら受け入れる
							TcpClient aClient = aListener.AcceptTcpClient();

							String aReceivedString;
							using (NetworkStream aNetworkStream = aClient.GetStream())
							{
								// ネットワークストリームの設定
								aNetworkStream.ReadTimeout = TCP_TIMEOUT;
								aNetworkStream.WriteTimeout = TCP_TIMEOUT;

								// クライアントから送られたデータを受信する
								Boolean aDisconnected = false;
								using (MemoryStream aMemoryStream = new MemoryStream())
								{
									Byte[] aReceived = new Byte[1024];
									Int32 aReceivedSize = 0;
									do
									{
										aReceivedSize = aNetworkStream.Read(aReceived, 0, aReceived.Length);
										if (aReceivedSize == 0)
										{
											// クライアントが切断したと判断
											aDisconnected = true;
											break;
										}

										// 受信したデータを蓄積する
										aMemoryStream.Write(aReceived, 0, aReceivedSize);
									} while (aNetworkStream.DataAvailable);

									//aReceivedString = Encoding.UTF8.GetString(aMemoryStream.GetBuffer(), 0, (Int32)aMemoryStream.Length);
									aReceivedString = HttpUtility.UrlDecode(aMemoryStream.GetBuffer(), 0, (Int32)aMemoryStream.Length, Encoding.UTF8);
								}

								if (!aDisconnected)
								{
									// クライアントに応答を返す
									String aBody = "OK";
									String aHeader = "HTTP/1.1 200 OK\n"
											+ "Content-Length: " + aBody.Length + "\n"
											+ "Content-Type: text/html\n\n";
									String aSendString = aHeader + aBody;
									Byte[] aSendBytes = Encoding.UTF8.GetBytes(aSendString);
									aNetworkStream.Write(aSendBytes, 0, aSendBytes.Length);
								}
							}

							// コメントを取り出す
							Int32 aCommentPos = aReceivedString.IndexOf(COMMENT_BEGIN_MARK);
							if (aCommentPos < 0)
							{
								throw new Exception("コメントデータが見つかりません。");
							}
							if (aCommentPos + COMMENT_BEGIN_MARK.Length == aReceivedString.Length)
							{
								throw new Exception("コメントデータが空です。");
							}
							String aComment = aReceivedString.Substring(aCommentPos + COMMENT_BEGIN_MARK.Length);


							// サーバーとの通信に成功したのでエラー表示解除
							ClearIsCommentReceiveError();

							// コメント発行
							CommentInfo aCommentInfo = AnalyzeExtendedCommentData(aComment);
							if (aCommentInfo != null)
							{
								mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメントを受信しました：" + aCommentInfo.Message);
								mFormViewer.AddComment(aCommentInfo);
							}

							// 閉じる
							aClient.Close();
						}
						catch (Exception oExcep)
						{
							mLogWriter.LogMessage(TraceEventType.Error, "コメント受信エラー（リトライします）：\n" + oExcep.Message);
							mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
							EnableIsCommentReceiveError();
						}

						aStopCancellationTokenSourceBak.Token.ThrowIfCancellationRequested();
						mClosingCancellationTokenSource.Token.ThrowIfCancellationRequested();
					}
				}
				catch (OperationCanceledException)
				{
					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "プッシュ受信処理を終了しました。");
				}
				catch (Exception oExcep)
				{
					mLogWriter.ShowLogMessage(TraceEventType.Error, "プッシュ受信エラー：\n" + oExcep.Message);
					mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
				}
				finally
				{
					if (aListener != null)
					{
						aListener.Stop();
					}
				}
			});

		}

		// --------------------------------------------------------------------
		// ファイル監視設定
		// --------------------------------------------------------------------
		private void SetFileSystemWatcherYukariConfig()
		{
			Boolean aIsWatch = true;
			String aConfigPath = String.Empty;

			if (mYukkoViewSettings.ServerSettingsType != ServerSettingsType.Auto)
			{
				// 手動設定の場合は監視しない
				aIsWatch = false;
			}
			else
			{
				// 設定ファイルのパスが有効な場合のみ監視する
				if (String.IsNullOrEmpty(mYukkoViewSettings.YukariConfigPath))
				{
					aIsWatch = false;
				}
				else
				{
					aConfigPath = Path.GetFullPath(mYukkoViewSettings.YukariConfigPath);
					if (!File.Exists(aConfigPath)
							|| String.IsNullOrEmpty(Path.GetDirectoryName(aConfigPath))
							|| String.IsNullOrEmpty(Path.GetFileName(aConfigPath)))
					{
						aIsWatch = false;
					}
				}
			}

			// 設定
			if (aIsWatch)
			{
				FileSystemWatcherYukariConfig.Path = Path.GetDirectoryName(aConfigPath);
				FileSystemWatcherYukariConfig.Filter = Path.GetFileName(aConfigPath);
				FileSystemWatcherYukariConfig.EnableRaisingEvents = true;
			}
			else
			{
				FileSystemWatcherYukariConfig.EnableRaisingEvents = false;
			}
		}

		// --------------------------------------------------------------------
		// 設定をコンポーネントに表示
		// --------------------------------------------------------------------
		private void SettingsToCompos()
		{
			// テストコメント
			TextBoxTest.Text = mYukkoViewSettings.TestComment;

			// コメントサーバー設定
			if (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto)
			{
				RadioButtonYukari.Checked = true;
			}
			else
			{
				RadioButtonManual.Checked = true;
			}
			UpdateServerComposEnabled();

			TextBoxYukariConfigPath.Text = mYukkoViewSettings.YukariConfigPath;
			TextBoxServerUrl.Text = mYukkoViewSettings.ServerUrl;
			TextBoxRoomName.Text = mYukkoViewSettings.RoomName;

			// ビューアウィンドウ
			SetComboBoxDisplay(mYukkoViewSettings.Display);
			SetComboBoxWindowState(mYukkoViewSettings.CommentWindowState);
		}

		// --------------------------------------------------------------------
		// コメント表示開始
		// --------------------------------------------------------------------
		private Task StartCommentAsync()
		{
			mIsRunning = true;
			mIsCommentReceiveError = false;
			UpdateStatusLabels();
			UpdatePlayerButtons();
			mFormViewer.Start();

			// 開始コメントを表示
			CommentInfo aCommentInfo = new CommentInfo();
			aCommentInfo.Message = "コメント表示を開始します";
			aCommentInfo.YukariSize = YukkoViewCommon.DEFAULT_YUKARI_FONT_SIZE;
			aCommentInfo.Color = Color.White;
			aCommentInfo.InitialTick = Environment.TickCount;
			mFormViewer.AddComment(aCommentInfo);

			// コメントサーバーからコメントを受信
			mStopCancellationTokenSource = new CancellationTokenSource();
			if (mYukkoViewSettings.ReceivePush)
			{
				return ReceivePushCommentAsync();
			}
			else
			{
				return DownloadCommentAsync();
			}
		}

		// --------------------------------------------------------------------
		// コメント表示停止
		// --------------------------------------------------------------------
		private Task StopCommentAsync()
		{
			return Task.Run(() =>
			{
				try
				{
					mIsRunning = false;
					UpdateStatusLabels();
					UpdatePlayerButtons();
					mFormViewer.Stop();

					mStopCancellationTokenSource.Cancel();

					if (mYukkoViewSettings.ReceivePush)
					{
						// ダミーコメントを投稿してプッシュ受信を終了させる
						TcpClient aClient = new TcpClient("localhost", mYukkoViewSettings.ReceivePushPort);
						using (NetworkStream aNetworkStream = aClient.GetStream())
						{
							aNetworkStream.ReadTimeout = TCP_TIMEOUT;
							aNetworkStream.WriteTimeout = TCP_TIMEOUT;
							Byte[] aSendBytes = Encoding.UTF8.GetBytes("X30FFFFFF1900-01-01 00:00:00 \t");
							aNetworkStream.Write(aSendBytes, 0, aSendBytes.Length);
						}
						aClient.Close();
					}

					mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "コメント表示終了");
				}
				catch (Exception oExcep)
				{
					mLogWriter.ShowLogMessage(TraceEventType.Error, "コメント表示終了時エラー：" + oExcep.Message, true);
				}
			});
		}

		// --------------------------------------------------------------------
		// ディスプレイ選択肢の更新（必要な場合のみ）
		// --------------------------------------------------------------------
		private void UpdateComboBoxDisplay()
		{
			// ディスプレイ
			Screen[] aScreens = Screen.AllScreens;
			if (ComboBoxDisplay.Items.Count == aScreens.Length)
			{
				return;
			}

			// バックアップ
			Int32 aIndexBak = ComboBoxDisplay.SelectedIndex;

			// 構築
			ComboBoxDisplay.Items.Clear();
			for (Int32 i = 0; i < aScreens.Length; i++)
			{
				String aText = "ディスプレイ " + (i + 1).ToString();
				if (aScreens[i].Primary)
				{
					aText += " （プライマリー）";
				}
				ComboBoxDisplay.Items.Add(aText);
			}
			ComboBoxDisplay.SelectedIndex = aIndexBak;
		}

		// --------------------------------------------------------------------
		// コメントサーバー設定コンポーネントの有効無効切替
		// --------------------------------------------------------------------
		private void UpdatePlayerButtons()
		{
			Invoke(new Action(() =>
			{
				ButtonStart.Enabled = !mIsRunning;
				ButtonStop.Enabled = mIsRunning;
				TextBoxTest.Enabled = mIsRunning;
				ButtonTest.Enabled = mIsRunning;
			}));
		}

		// --------------------------------------------------------------------
		// コメントサーバー設定コンポーネントの有効無効切替
		// --------------------------------------------------------------------
		private void UpdateServerComposEnabled()
		{
			// 自動
			TextBoxYukariConfigPath.Enabled = (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto);
			ButtonBrowse.Enabled = (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto);

			// 手動
			TextBoxServerUrl.Enabled = (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Manual);
			TextBoxRoomName.Enabled = (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Manual);
		}

		// --------------------------------------------------------------------
		// ステータス系ラベルの更新
		// ＜サブスレッドから呼びだし可能なコントロール操作関数＞
		// --------------------------------------------------------------------
		private void UpdateStatusLabels()
		{
			Invoke(new Action(() =>
			{
				// クリア
				ToolTipControl.SetToolTip(LabelStatus, null);

				// 更新
				if (mIsRunning)
				{
					if (mIsCommentReceiveError)
					{
						LabelLamp.ForeColor = COLOR_STATUS_ERROR;
						LabelStatus.Text = "コメントサーバーと通信できません";
					}
					else
					{
						LabelLamp.ForeColor = COLOR_STATUS_RUNNING;
						LabelStatus.Text = "コメント表示動作中";
					}
				}
				else
				{
					if (mIsYukariConfigError)
					{
						LabelLamp.ForeColor = COLOR_STATUS_ERROR;
						LabelStatus.Text = "ゆかりの設定を読み込めませんでした";
						ToolTipControl.SetToolTip(LabelStatus, mYukariConfigErrorMessage);
					}
					else
					{
						LabelLamp.ForeColor = COLOR_STATUS_STOP;
						LabelStatus.Text = "開始ボタンをクリックして下さい";
					}
				}
			}));
		}


		// ====================================================================
		// IDE 生成イベントハンドラー
		// ====================================================================

		private void FormControl_Load(object sender, EventArgs e)
		{
			try
			{
				// メンバ変数の初期化
				Init();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "起動時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FormControl_Shown(object sender, EventArgs e)
		{
			try
			{
				// 更新起動時とパス変更時の記録
				// 新規起動時は、両フラグが立つのでダブらないように注意
				Boolean aVerChanged = mYukkoViewSettings.PrevLaunchVer != YukkoViewCommon.APP_VER;
				if (aVerChanged)
				{
					if (String.IsNullOrEmpty(mYukkoViewSettings.PrevLaunchVer))
					{
						mLogWriter.LogMessage(TraceEventType.Information, "新規起動：" + YukkoViewCommon.APP_VER);
					}
					else
					{
						mLogWriter.LogMessage(TraceEventType.Information, "更新起動：" + mYukkoViewSettings.PrevLaunchVer + "→" + YukkoViewCommon.APP_VER);
					}
				}
				Boolean aPathChanged = (String.Compare(mYukkoViewSettings.PrevLaunchPath, Application.ExecutablePath, true) != 0);
				if (aPathChanged && !String.IsNullOrEmpty(mYukkoViewSettings.PrevLaunchPath))
				{
					mLogWriter.LogMessage(TraceEventType.Information, "パス変更起動：" + mYukkoViewSettings.PrevLaunchPath + "→" + Application.ExecutablePath);
				}

				// 更新起動時とパス変更時の処理
				if (aVerChanged || aPathChanged)
				{
					YukkoViewCommon.LogEnvironmentInfo();
				}
				if (aVerChanged)
				{
					NewVersionLaunched();
				}

				// 必要に応じてちょちょいと自動更新を起動
				if (mYukkoViewSettings.IsCheckRssNeeded())
				{
					if (YukkoViewCommon.LaunchUpdater(true, false, IntPtr.Zero, false, false))
					{
						mYukkoViewSettings.RssCheckDate = DateTime.Now.Date;
						mYukkoViewSettings.Save();
					}
				}

				Common.CloseIfNet45IsnotInstalled(this, YukkoViewCommon.APP_NAME_J, mLogWriter);

				// ゆかり設定読み込み
				if (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto)
				{
					LoadYukariConfig();
				}

				// 設定を表示
				SettingsToCompos();

				// 状態表示
				UpdateStatusLabels();
				UpdatePlayerButtons();

				// 設定ファイル変更監視
				SetFileSystemWatcherYukariConfig();

				// ビューアウィンドウ表示
				mFormViewer.Show();
				MoveFormViewer();

				// 自動開始（async を待機しない）
				if (mYukkoViewSettings.AutoRun)
				{
					Task aSuppressWarning = StartCommentAsync();
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "メインウィンドウ表示時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}

		}

		private void FormControl_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				// 終了時タスクキャンセル
				mClosingCancellationTokenSource.Cancel();
				mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "終了処理中...");

				mFormViewer.Dispose();

				// 終了時の状態
				mYukkoViewSettings.PrevLaunchPath = Application.ExecutablePath;
				mYukkoViewSettings.PrevLaunchVer = YukkoViewCommon.APP_VER;
				mYukkoViewSettings.DesktopBounds = DesktopBounds;
				mYukkoViewSettings.Save();

				mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "終了しました：" + YukkoViewCommon.APP_NAME_J + " "
						+ YukkoViewCommon.APP_VER + " --------------------");
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "終了時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonMoveWindow_Click(object sender, EventArgs e)
		{
			try
			{
				MoveFormViewer();

				// 接続されているディスプレイの数が変更されている場合があるので、選択肢を更新しておく
				UpdateComboBoxDisplay();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "移動時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ComboBoxDisplay_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				mYukkoViewSettings.Display = ComboBoxDisplay.SelectedIndex;
				mYukkoViewSettings.Save();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ディスプレイ選択時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ComboBoxWindowState_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (ComboBoxWindowState.SelectedIndex == 0)
				{
					mYukkoViewSettings.CommentWindowState = FormWindowState.Maximized;
				}
				else
				{
					mYukkoViewSettings.CommentWindowState = FormWindowState.Normal;
				}
				mYukkoViewSettings.Save();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ウィンドウ状態選択時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonStart_Click(object sender, EventArgs e)
		{
			try
			{
				// async を待機しない
				Task aSuppressWarning = StartCommentAsync();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "コメント表示開始時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonStop_Click(object sender, EventArgs e)
		{
			try
			{
				// async を待機しない
				Task aSuppressWarning = StopCommentAsync();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "コメント表示終了時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonTest_Click(object sender, EventArgs e)
		{
			try
			{
				CommentInfo aCommentInfo = new CommentInfo();

				aCommentInfo.Message = TextBoxTest.Text;
				aCommentInfo.YukariSize = YukkoViewCommon.DEFAULT_YUKARI_FONT_SIZE;
				aCommentInfo.Color = COLOR_TEST_COMMENTS[mTestCommentColorIndex];
				aCommentInfo.InitialTick = Environment.TickCount;

				// 色番号調整
				mTestCommentColorIndex++;
				if (mTestCommentColorIndex >= COLOR_TEST_COMMENTS.Length)
				{
					mTestCommentColorIndex = 0;
				}

				// コメント追加
				mFormViewer.AddComment(aCommentInfo);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "テスト表示時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void TextBoxYukariConfigPath_TextChanged(object sender, EventArgs e)
		{
			try
			{
				// 設定変更
				mYukkoViewSettings.YukariConfigPath = TextBoxYukariConfigPath.Text;

				// ゆかり設定読み込み
				if (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto)
				{
					LoadYukariConfig();
					SettingsToCompos();
				}
				mYukkoViewSettings.Save();

				// 状態表示
				UpdateStatusLabels();
				SetFileSystemWatcherYukariConfig();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "設定ファイル名入力時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialogYukariConfigPath.FileName = null;
				if (OpenFileDialogYukariConfigPath.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				TextBoxYukariConfigPath.Text = OpenFileDialogYukariConfigPath.FileName;
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "設定ファイル参照時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void TextBoxServerUrl_TextChanged(object sender, EventArgs e)
		{
			try
			{
				mYukkoViewSettings.ServerUrl = TextBoxServerUrl.Text;
				mYukkoViewSettings.Save();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "コメントサーバー入力時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void TextBoxRoomName_TextChanged(object sender, EventArgs e)
		{
			try
			{
				mYukkoViewSettings.RoomName = TextBoxRoomName.Text;
				mYukkoViewSettings.Save();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ルーム名入力時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void FileSystemWatcherYukariConfig_Changed(object sender, FileSystemEventArgs e)
		{
			try
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "FileSystemWatcherYukariConfig_Changed()");

				// ゆかり設定読み込み
				if (mYukkoViewSettings.ServerSettingsType == ServerSettingsType.Auto)
				{
					LoadYukariConfig();
				}

				// 設定を表示
				SettingsToCompos();

				// お知らせ
				LabelLamp.ForeColor = COLOR_STATUS_NOTICE;
				LabelStatus.Text = "ゆかりの設定更新を反映しました";
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ゆかり設定ファイル変更検出時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonSettings_Click(object sender, EventArgs e)
		{
			try
			{
				Boolean aIsRunningBak = mIsRunning;

				using (FormSettings aFormSettings = new FormSettings(mYukkoViewSettings, mLogWriter))
				{
					// 設定時はビューアウィンドウを非表示にする、コメントは停止する
					mFormViewer.Hide();
					if (aIsRunningBak)
					{
						// async を待機しない
						Task aSuppressWarning = StopCommentAsync();
					}
					DialogResult aResult = aFormSettings.ShowDialog(this);
					if (aIsRunningBak)
					{
						// async を待機しない
						Task aSuppressWarning = StartCommentAsync();
					}
					mFormViewer.Show();
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "環境設定時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ButtonHelp_Click(object sender, EventArgs e)
		{
			try
			{
				ContextMenuHelp.Show(Cursor.Position);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ヘルプメニュー表示時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void バージョン情報ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (FormAbout aFormAbout = new FormAbout(mLogWriter))
				{
					// バージョン情報表示時はビューアウィンドウを非表示にする
					mFormViewer.Hide();
					aFormAbout.ShowDialog(this);
					mFormViewer.Show();
				}
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "バージョン情報メニュー実行時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void ヘルプHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				YukkoViewCommon.ShowHelp();
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "ヘルプメニュー実行時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		private void 改訂履歴UToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + FILE_NAME_HISTORY);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "改訂履歴を表示できませんでした：\n" + FILE_NAME_HISTORY);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}
	} // public partial class FormControl

} // namespace YukkoView
