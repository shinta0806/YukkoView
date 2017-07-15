// ============================================================================
// 
// ゆっこビューの設定を管理
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace YukkoView.Shared
{
	// 設定の保存場所を Application.UserAppDataPath 配下にする
	[SettingsProvider(typeof(ApplicationNameSettingsProvider))]
	public class YukkoViewSettings : ApplicationSettingsBase
	{
		// ====================================================================
		// public プロパティ
		// ====================================================================

		// --------------------------------------------------------------------
		// コメント設定
		// --------------------------------------------------------------------

		private const String KEY_NAME_SERVER_SETTINGS_TYPE = "ServerSettingsType";
		[UserScopedSetting]
		public ServerSettingsType ServerSettingsType
		{
			get
			{
				return (ServerSettingsType)this[KEY_NAME_SERVER_SETTINGS_TYPE];
			}
			set
			{
				this[KEY_NAME_SERVER_SETTINGS_TYPE] = value;
			}
		}

		private const String KEY_NAME_YUKARI_CONFIG_PATH = "YukariConfigPath";
		[UserScopedSetting]
		[DefaultSettingValue(@"..\config.ini")]
		public String YukariConfigPath
		{
			get
			{
				return (String)this[KEY_NAME_YUKARI_CONFIG_PATH];
			}
			set
			{
				this[KEY_NAME_YUKARI_CONFIG_PATH] = value;
			}
		}

		private const String KEY_NAME_SERVER_URL = "ServerUrl";
		[UserScopedSetting]
		public String ServerUrl
		{
			get
			{
				return (String)this[KEY_NAME_SERVER_URL];
			}
			set
			{
				this[KEY_NAME_SERVER_URL] = value;
			}
		}

		private const String KEY_NAME_ROOM_NAME = "RoomName";
		[UserScopedSetting]
		public String RoomName
		{
			get
			{
				return (String)this[KEY_NAME_ROOM_NAME];
			}
			set
			{
				this[KEY_NAME_ROOM_NAME] = value;
			}
		}

		private const String KEY_NAME_DISPLAY = "Display";
		[UserScopedSetting]
		public Int32 Display
		{
			get
			{
				return (Int32)this[KEY_NAME_DISPLAY];
			}
			set
			{
				this[KEY_NAME_DISPLAY] = value;
			}
		}

		private const String KEY_NAME_COMMENT_WINDOW_STATE = "CommentWindowState";
		[UserScopedSetting]
		[DefaultSettingValue("2")]  // FormWindowState.Maximized
		public FormWindowState CommentWindowState
		{
			get
			{
				return (FormWindowState)this[KEY_NAME_COMMENT_WINDOW_STATE];
			}
			set
			{
				this[KEY_NAME_COMMENT_WINDOW_STATE] = value;
			}
		}

		// --------------------------------------------------------------------
		// 環境設定
		// --------------------------------------------------------------------

		private const String KEY_NAME_AUTO_RUN = "AutoRun";
		[UserScopedSetting]
		[DefaultSettingValue(Common.BOOLEAN_STRING_FALSE)]
		public Boolean AutoRun
		{
			get
			{
				return (Boolean)this[KEY_NAME_AUTO_RUN];
			}
			set
			{
				this[KEY_NAME_AUTO_RUN] = value;
			}
		}

		private const String KEY_NAME_ENABLE_MARGIN = "EnableMargin";
		[UserScopedSetting]
		[DefaultSettingValue(Common.BOOLEAN_STRING_FALSE)]
		public Boolean EnableMargin
		{
			get
			{
				return (Boolean)this[KEY_NAME_ENABLE_MARGIN];
			}
			set
			{
				this[KEY_NAME_ENABLE_MARGIN] = value;
			}
		}

		private const String KEY_NAME_MARGIN_PERCENT = "MarginPercent";
		[UserScopedSetting]
		[DefaultSettingValue("10")]
		public Int32 MarginPercent
		{
			get
			{
				return (Int32)this[KEY_NAME_MARGIN_PERCENT];
			}
			set
			{
				this[KEY_NAME_MARGIN_PERCENT] = value;
			}
		}

		private const String KEY_NAME_INTERVAL = "Interval";
		[UserScopedSetting]
		[DefaultSettingValue("1000")]
		public Int32 Interval
		{
			get
			{
				return (Int32)this[KEY_NAME_INTERVAL];
			}
			set
			{
				this[KEY_NAME_INTERVAL] = value;
			}
		}

		// --------------------------------------------------------------------
		// メンテナンス
		// --------------------------------------------------------------------

		private const String KEY_NAME_CHECK_RSS = "CheckRSS";
		[UserScopedSetting]
		[DefaultSettingValue(Common.BOOLEAN_STRING_TRUE)]
		public Boolean CheckRss
		{
			get
			{
				return (Boolean)this[KEY_NAME_CHECK_RSS];
			}
			set
			{
				this[KEY_NAME_CHECK_RSS] = value;
			}
		}

		// --------------------------------------------------------------------
		// 終了時の状態
		// --------------------------------------------------------------------

		private const String KEY_NAME_PREV_LAUNCH_VER = "PrevLaunchVer";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public String PrevLaunchVer
		{
			get
			{
				return (String)this[KEY_NAME_PREV_LAUNCH_VER];
			}
			set
			{
				this[KEY_NAME_PREV_LAUNCH_VER] = value;
			}
		}

		private const String KEY_NAME_PREV_LAUNCH_PATH = "PrevLaunchPath";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public String PrevLaunchPath
		{
			get
			{
				return (String)this[KEY_NAME_PREV_LAUNCH_PATH];
			}
			set
			{
				this[KEY_NAME_PREV_LAUNCH_PATH] = value;
			}
		}

		private const String KEY_NAME_DESKTOP_BOUNDS = "DesktopBounds";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public Rectangle DesktopBounds
		{
			get
			{
				return (Rectangle)this[KEY_NAME_DESKTOP_BOUNDS];
			}
			set
			{
				this[KEY_NAME_DESKTOP_BOUNDS] = value;
			}
		}

		private const String KEY_NAME_RSS_CHECK_DATE = "RSSCheckDate";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public DateTime RssCheckDate
		{
			get
			{
				return (DateTime)this[KEY_NAME_RSS_CHECK_DATE];
			}
			set
			{
				this[KEY_NAME_RSS_CHECK_DATE] = value;
			}
		}

		private const String KEY_NAME_TEST_COMMENT = "TestComment";
		[UserScopedSetting]
		[DefaultSettingValue("テスト中～～～")]
		public String TestComment
		{
			get
			{
				return (String)this[KEY_NAME_TEST_COMMENT];
			}
			set
			{
				this[KEY_NAME_TEST_COMMENT] = value;
			}
		}



		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 簡易コピー
		// --------------------------------------------------------------------
		public YukkoViewSettings Clone()
		{
			return (YukkoViewSettings)MemberwiseClone();
		}

		// --------------------------------------------------------------------
		// RSS の確認が必要かどうか
		// --------------------------------------------------------------------
		public Boolean IsCheckRssNeeded()
		{
			if (!CheckRss)
			{
				return false;
			}
			DateTime aEmptyDate = new DateTime();
			TimeSpan aDay3 = new TimeSpan(3, 0, 0, 0);
			return RssCheckDate == aEmptyDate || DateTime.Now.Date - RssCheckDate >= aDay3;
		}

	} // public class YukariCommentViewerSettings

} // namespace YukkoView.Shared


