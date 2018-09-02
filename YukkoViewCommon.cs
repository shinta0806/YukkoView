// ============================================================================
// 
// ゆっこビュー共通で使用する、定数・関数
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace YukkoView.Shared
{
	// ====================================================================
	// public 列挙型
	// ====================================================================

	// --------------------------------------------------------------------
	// コメントサーバー設定方法
	// --------------------------------------------------------------------
	public enum ServerSettingsType
	{
		Auto,
		Manual,
	}

	// ====================================================================
	// ゆっこビュー共通
	// ====================================================================

	public class YukkoViewCommon
	{
		// ====================================================================
		// public 定数
		// ====================================================================

		// --------------------------------------------------------------------
		// アプリの基本情報
		// --------------------------------------------------------------------
		public const String APP_ID = "YukkoView";
		public const String APP_NAME_J = "ゆっこビュー";
		public const String APP_VER = "Ver 2.62";
		public const String COPYRIGHT_J = "Copyright (C) 2017-2018 by SHINTA";

		// --------------------------------------------------------------------
		// 環境設定
		// --------------------------------------------------------------------

		// コメント表示上下マージン
		public const Int32 MARGIN_PERCENT_MIN = 1;
		public const Int32 MARGIN_PERCENT_MAX = 25;

		// 同時に表示するコメントの最大数
		public const Int32 NUM_DISPLAY_COMMENTS_MAX = 7;

		// --------------------------------------------------------------------
		// ファイル名・フォルダー名
		// --------------------------------------------------------------------

		// ちょちょいと自動更新の設定ファイル
		public const String FILE_NAME_RSS_INI = "YukkoView_Latest.config";

		// ちょちょいと自動更新のフォルダー名
		public const String FOLDER_NAME_CUPDATER = "Updater\\";

		// 設定用フォルダー名
		public const String FOLDER_NAME_YUKKOVIEW = APP_ID + "\\";

		// --------------------------------------------------------------------
		// ゆかり関連
		// --------------------------------------------------------------------

		// デフォルトコメントフォントサイズ（ゆかりから送られてくるサイズの中）
		public const Int32 DEFAULT_YUKARI_FONT_SIZE = 3;

		// --------------------------------------------------------------------
		// 描画関連
		// --------------------------------------------------------------------

		// 縁取りの幅
		public const Int32 EDGE_WIDTH = 6;

		// --------------------------------------------------------------------
		// その他
		// --------------------------------------------------------------------

		// 連続投稿防止間隔 [ms]
		public const Int32 CONTINUOUS_PREVENT_TIME = 5000;

		// ====================================================================
		// public プロパティー
		// ====================================================================

		// ログ
		public static LogWriter LogWriter { get; set; }

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// ちょちょいと自動更新を起動
		// --------------------------------------------------------------------
		public static Boolean LaunchUpdater(Boolean oCheckLatest, Boolean oForceShow, IntPtr oHWnd, Boolean oClearUpdateCache, Boolean oForceInstall)
		{
			// 固定部分
			UpdaterLauncher aUpdaterLauncher = new UpdaterLauncher();
			aUpdaterLauncher.ID = "YukkoView";
			aUpdaterLauncher.Name = YukkoViewCommon.APP_NAME_J;
			aUpdaterLauncher.Wait = 3;
			aUpdaterLauncher.UpdateRss = "http://shinta.coresv.com/soft/YukkoView_AutoUpdate.xml";
			aUpdaterLauncher.CurrentVer = YukkoViewCommon.APP_VER;

			// 変動部分
			if (oCheckLatest)
			{
				aUpdaterLauncher.LatestRss = "http://shinta.coresv.com/soft/YukkoView_JPN.xml";
			}
			aUpdaterLauncher.LogWriter = LogWriter;
			aUpdaterLauncher.ForceShow = oForceShow;
			aUpdaterLauncher.NotifyHWnd = oHWnd;
			aUpdaterLauncher.ClearUpdateCache = oClearUpdateCache;
			aUpdaterLauncher.ForceInstall = oForceInstall;

			// 起動
			return aUpdaterLauncher.Launch(oForceShow);
		}

		// --------------------------------------------------------------------
		// 環境情報をログする
		// --------------------------------------------------------------------
		public static void LogEnvironmentInfo()
		{
			SystemEnvironment aSE = new SystemEnvironment();
			aSE.LogEnvironment(LogWriter);
		}

		// --------------------------------------------------------------------
		// ちょちょいと自動更新の更新情報を格納している ini ファイルのパス
		// --------------------------------------------------------------------
		public static String RssIniPath()
		{
			String aSettingsPath = SettingsPath();
			return Path.GetDirectoryName(aSettingsPath.Substring(0, aSettingsPath.Length - 1)) + "\\" + FOLDER_NAME_CUPDATER + FILE_NAME_RSS_INI;
		}

		// --------------------------------------------------------------------
		// 設定保存フォルダのパス（末尾 '\\'）
		// 存在しない場合は作成される
		// --------------------------------------------------------------------
		public static String SettingsPath()
		{
			return Path.GetDirectoryName(Application.UserAppDataPath) + "\\";
		}

		// --------------------------------------------------------------------
		// ヘルプの表示
		// --------------------------------------------------------------------
		public static void ShowHelp(String oAnchor = null)
		{
			String aHelpPath = null;

			try
			{
				String aHelpPathBase = Path.GetDirectoryName(Application.ExecutablePath) + "\\";

				// アンカーが指定されている場合は状況依存型ヘルプを表示
				if (!String.IsNullOrEmpty(oAnchor))
				{
					aHelpPath = aHelpPathBase + FOLDER_NAME_HELP_PARTS + FILE_NAME_HELP_PREFIX + "_" + oAnchor + Common.FILE_EXT_HTML;
					try
					{
						Process.Start(aHelpPath);
						return;
					}
					catch (Exception oExcep)
					{
						LogWriter.ShowLogMessage(TraceEventType.Error, "状況に応じたヘルプを表示できませんでした：\n" + oExcep.Message + "\n" + aHelpPath
								+ "\n通常のヘルプを表示します。");
					}
				}

				// アンカーが指定されていない場合・状況依存型ヘルプを表示できなかった場合は通常のヘルプを表示
				aHelpPath = aHelpPathBase + FILE_NAME_HELP_PREFIX + Common.FILE_EXT_HTML;
				Process.Start(aHelpPath);
			}
			catch (Exception oExcep)
			{
				LogWriter.ShowLogMessage(TraceEventType.Error, "ヘルプを表示できませんでした。\n" + oExcep.Message + "\n" + aHelpPath);
			}
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		// --------------------------------------------------------------------
		// ファイル名
		// --------------------------------------------------------------------
		private const String FILE_NAME_HELP_PREFIX = APP_ID + "_JPN";
		private const String FOLDER_NAME_HELP_PARTS = "HelpParts\\";



	} // public class YukkoViewCommon

	// ====================================================================
	// コメント表示用情報
	// ====================================================================

	public class CommentInfo : IDisposable
	{
		// ====================================================================
		// public プロパティー
		// ====================================================================

		// --------------------------------------------------------------------
		// 基本情報
		// --------------------------------------------------------------------

		// コメント内容
		public String Message { get; set; }

		// サイズ（ゆかり指定サイズ）
		public Int32 YukariSize { get; set; }

		// 色
		public Color Color { get; set; }

		// コメントを取得した時刻
		public Int32 InitialTick { get; set; }

		// --------------------------------------------------------------------
		// ビューア側で計算する情報
		// --------------------------------------------------------------------

		// 移動速度 [px/s]
		public Int32 Speed { get; set; }

		// 描画指定位置
		public Int32 SpecifyLeft { get; set; }

		// --------------------------------------------------------------------
		// 描画情報
		// --------------------------------------------------------------------

		// コメント描画用グラフィックスパス
		public GraphicsPath MessagePath
		{
			get
			{
				return mMessagePath;
			}
			set
			{
				if (mMessagePath != null)
				{
					mMessagePath.Dispose();
				}
				mMessagePath = value;
			}
		}

		// ブラシ
		public SolidBrush Brush
		{
			get
			{
				return mBrush;
			}
			set
			{
				if (mBrush != null)
				{
					mBrush.Dispose();
				}
				mBrush = value;
			}
		}

		// 表示される位置（縁取り込み）
		// ※位置指定は MessagePath の Transform で行う
		public Int32 Top
		{
			get
			{
				return (Int32)(mMessagePath.GetBounds().Top - YukkoViewCommon.EDGE_WIDTH / 2);
			}
		}

		public Int32 Right
		{
			get
			{
				return (Int32)(mMessagePath.GetBounds().Right + YukkoViewCommon.EDGE_WIDTH / 2);
			}
		}

		public Int32 Bottom
		{
			get
			{
				return (Int32)(mMessagePath.GetBounds().Bottom + YukkoViewCommon.EDGE_WIDTH / 2);
			}
		}

		// 表示されるサイズ（縁取り込み）
		public Int32 Width
		{
			get
			{
				return (Int32)(mMessagePath.GetBounds().Width + YukkoViewCommon.EDGE_WIDTH);
			}
		}

		public Int32 Height
		{
			get
			{
				return (Int32)(mMessagePath.GetBounds().Height + YukkoViewCommon.EDGE_WIDTH);
			}
		}

		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public CommentInfo()
		{
		}

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 基本情報の比較
		// ＜返値＞ 基本情報が全て等しければ true
		// --------------------------------------------------------------------
		public Boolean CompareBase(CommentInfo oComp)
		{
			return oComp != null
					&& Message == oComp.Message
					&& YukariSize == oComp.YukariSize
					&& Color == oComp.Color;
		}

		// --------------------------------------------------------------------
		// 後始末
		// --------------------------------------------------------------------
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// ====================================================================
		// protected メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 後始末
		// --------------------------------------------------------------------
		protected virtual void Dispose(bool oDisposing)
		{
			if (oDisposing)
			{
				if (mMessagePath != null)
				{
					mMessagePath.Dispose();
				}
				if (mBrush != null)
				{
					mBrush.Dispose();
				}
			}
		}

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// MessagePath
		private GraphicsPath mMessagePath;

		// Brush
		private SolidBrush mBrush;

	} // public class CommentInfo



} // YukkoView.Shared
