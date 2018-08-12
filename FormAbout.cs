// ============================================================================
// 
// バージョン情報フォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
//
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using YukkoView.Shared;

namespace YukkoView
{
	public partial class FormAbout : Form
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormAbout(LogWriter oLogWriter)
		{
			InitializeComponent();

			mLogWriter = oLogWriter;
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// LinkLabel のクリックを集約
		// --------------------------------------------------------------------
		private void LinkLabels_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string aLink = String.Empty;

			try
			{
				// MSDN を見ると e.Link.LinkData がリンク先のように読めなくもないが、実際には
				// 値が入っていないので sender をキャストしてリンク先を取得する
				e.Link.Visited = true;
				aLink = ((LinkLabel)sender).Text;
				Process.Start(aLink);
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "リンク先を表示できませんでした。\n" + aLink);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}

		// ====================================================================
		// IDE 生成イベントハンドラー
		// ====================================================================

		private void FormAbout_Load(object sender, EventArgs e)
		{
			try
			{
				// 表示
				Text = YukkoViewCommon.APP_NAME_J + "のバージョン情報";
#if DEBUG
				Text = "［デバッグ］" + Text;
#endif
				LabelAppName.Text = YukkoViewCommon.APP_NAME_J;
				LabelAppVer.Text = YukkoViewCommon.APP_VER;
				LabelCopyright.Text = YukkoViewCommon.COPYRIGHT_J;

				Common.CascadeForm(this);

				// コントロール
				ActiveControl = ButtonOK;
			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "バージョン情報ロード時エラー：\n" + oExcep.Message);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "　スタックトレース：\n" + oExcep.StackTrace);
			}
		}


	}
}
