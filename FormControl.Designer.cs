namespace YukkoView
{
	partial class FormControl
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormControl));
			this.LabelLamp = new System.Windows.Forms.Label();
			this.LabelStatus = new System.Windows.Forms.Label();
			this.TextBoxTest = new System.Windows.Forms.TextBox();
			this.ButtonTest = new System.Windows.Forms.Button();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ButtonBrowse = new System.Windows.Forms.Button();
			this.TextBoxYukariConfigPath = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.TextBoxRoomName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.TextBoxServerUrl = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.RadioButtonManual = new System.Windows.Forms.RadioButton();
			this.RadioButtonYukari = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.ComboBoxDisplay = new System.Windows.Forms.ComboBox();
			this.ButtonMoveWindow = new System.Windows.Forms.Button();
			this.ComboBoxWindowState = new System.Windows.Forms.ComboBox();
			this.ButtonStop = new System.Windows.Forms.Button();
			this.ButtonStart = new System.Windows.Forms.Button();
			this.OpenFileDialogYukariConfigPath = new System.Windows.Forms.OpenFileDialog();
			this.FileSystemWatcherYukariConfig = new System.IO.FileSystemWatcher();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ButtonSettings = new System.Windows.Forms.Button();
			this.ButtonHelp = new System.Windows.Forms.Button();
			this.ContextMenuHelp = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ヘルプHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.改訂履歴UToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.バージョン情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolTipControl = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcherYukariConfig)).BeginInit();
			this.ContextMenuHelp.SuspendLayout();
			this.SuspendLayout();
			// 
			// LabelLamp
			// 
			this.LabelLamp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LabelLamp.Location = new System.Drawing.Point(16, 16);
			this.LabelLamp.Name = "LabelLamp";
			this.LabelLamp.Size = new System.Drawing.Size(32, 16);
			this.LabelLamp.TabIndex = 0;
			this.LabelLamp.Text = "●";
			this.LabelLamp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelStatus
			// 
			this.LabelStatus.Location = new System.Drawing.Point(48, 16);
			this.LabelStatus.Name = "LabelStatus";
			this.LabelStatus.Size = new System.Drawing.Size(328, 16);
			this.LabelStatus.TabIndex = 1;
			this.LabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TextBoxTest
			// 
			this.TextBoxTest.Location = new System.Drawing.Point(16, 88);
			this.TextBoxTest.Name = "TextBoxTest";
			this.TextBoxTest.Size = new System.Drawing.Size(256, 19);
			this.TextBoxTest.TabIndex = 4;
			// 
			// ButtonTest
			// 
			this.ButtonTest.Location = new System.Drawing.Point(280, 84);
			this.ButtonTest.Name = "ButtonTest";
			this.ButtonTest.Size = new System.Drawing.Size(96, 28);
			this.ButtonTest.TabIndex = 5;
			this.ButtonTest.Text = "テスト表示 (&T)";
			this.ButtonTest.UseVisualStyleBackColor = true;
			this.ButtonTest.Click += new System.EventHandler(this.ButtonTest_Click);
			// 
			// panel5
			// 
			this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel5.Location = new System.Drawing.Point(0, 132);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(500, 5);
			this.panel5.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 128);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 12);
			this.label1.TabIndex = 7;
			this.label1.Text = "　コメント設定　";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ButtonBrowse);
			this.groupBox1.Controls.Add(this.TextBoxYukariConfigPath);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.TextBoxRoomName);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.TextBoxServerUrl);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.RadioButtonManual);
			this.groupBox1.Controls.Add(this.RadioButtonYukari);
			this.groupBox1.Location = new System.Drawing.Point(16, 148);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 164);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// ButtonBrowse
			// 
			this.ButtonBrowse.Location = new System.Drawing.Point(264, 40);
			this.ButtonBrowse.Name = "ButtonBrowse";
			this.ButtonBrowse.Size = new System.Drawing.Size(80, 28);
			this.ButtonBrowse.TabIndex = 3;
			this.ButtonBrowse.Text = "参照 (&B)";
			this.ButtonBrowse.UseVisualStyleBackColor = true;
			this.ButtonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
			// 
			// TextBoxYukariConfigPath
			// 
			this.TextBoxYukariConfigPath.Location = new System.Drawing.Point(144, 44);
			this.TextBoxYukariConfigPath.Name = "TextBoxYukariConfigPath";
			this.TextBoxYukariConfigPath.Size = new System.Drawing.Size(112, 19);
			this.TextBoxYukariConfigPath.TabIndex = 2;
			this.TextBoxYukariConfigPath.TextChanged += new System.EventHandler(this.TextBoxYukariConfigPath_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(32, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 20);
			this.label7.TabIndex = 1;
			this.label7.Text = "設定ファイル (&F)：";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TextBoxRoomName
			// 
			this.TextBoxRoomName.Location = new System.Drawing.Point(144, 132);
			this.TextBoxRoomName.Name = "TextBoxRoomName";
			this.TextBoxRoomName.Size = new System.Drawing.Size(200, 19);
			this.TextBoxRoomName.TabIndex = 8;
			this.TextBoxRoomName.TextChanged += new System.EventHandler(this.TextBoxRoomName_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 132);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 20);
			this.label3.TabIndex = 7;
			this.label3.Text = "ルーム名 (&R)：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TextBoxServerUrl
			// 
			this.TextBoxServerUrl.Location = new System.Drawing.Point(144, 104);
			this.TextBoxServerUrl.Name = "TextBoxServerUrl";
			this.TextBoxServerUrl.Size = new System.Drawing.Size(200, 19);
			this.TextBoxServerUrl.TabIndex = 6;
			this.TextBoxServerUrl.TextChanged += new System.EventHandler(this.TextBoxServerUrl_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 20);
			this.label2.TabIndex = 5;
			this.label2.Text = "コメントサーバー (&C)：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RadioButtonManual
			// 
			this.RadioButtonManual.AutoSize = true;
			this.RadioButtonManual.Location = new System.Drawing.Point(16, 80);
			this.RadioButtonManual.Name = "RadioButtonManual";
			this.RadioButtonManual.Size = new System.Drawing.Size(102, 16);
			this.RadioButtonManual.TabIndex = 4;
			this.RadioButtonManual.TabStop = true;
			this.RadioButtonManual.Text = "手動で設定 (&M)";
			this.RadioButtonManual.UseVisualStyleBackColor = true;
			this.RadioButtonManual.CheckedChanged += new System.EventHandler(this.RadioButtonConfig_CheckedChanged);
			// 
			// RadioButtonYukari
			// 
			this.RadioButtonYukari.AutoSize = true;
			this.RadioButtonYukari.Location = new System.Drawing.Point(16, 20);
			this.RadioButtonYukari.Name = "RadioButtonYukari";
			this.RadioButtonYukari.Size = new System.Drawing.Size(162, 16);
			this.RadioButtonYukari.TabIndex = 0;
			this.RadioButtonYukari.TabStop = true;
			this.RadioButtonYukari.Text = "ゆかりの設定を自動取得 (&A)";
			this.RadioButtonYukari.UseVisualStyleBackColor = true;
			this.RadioButtonYukari.CheckedChanged += new System.EventHandler(this.RadioButtonConfig_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 328);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 20);
			this.label4.TabIndex = 9;
			this.label4.Text = "ビューアウィンドウ (&V)：";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ComboBoxDisplay
			// 
			this.ComboBoxDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBoxDisplay.FormattingEnabled = true;
			this.ComboBoxDisplay.Location = new System.Drawing.Point(160, 328);
			this.ComboBoxDisplay.Name = "ComboBoxDisplay";
			this.ComboBoxDisplay.Size = new System.Drawing.Size(200, 20);
			this.ComboBoxDisplay.TabIndex = 10;
			this.ComboBoxDisplay.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDisplay_SelectedIndexChanged);
			// 
			// ButtonMoveWindow
			// 
			this.ButtonMoveWindow.Location = new System.Drawing.Point(280, 356);
			this.ButtonMoveWindow.Name = "ButtonMoveWindow";
			this.ButtonMoveWindow.Size = new System.Drawing.Size(80, 28);
			this.ButtonMoveWindow.TabIndex = 12;
			this.ButtonMoveWindow.Text = "移動 (&O)";
			this.ButtonMoveWindow.UseVisualStyleBackColor = true;
			this.ButtonMoveWindow.Click += new System.EventHandler(this.ButtonMoveWindow_Click);
			// 
			// ComboBoxWindowState
			// 
			this.ComboBoxWindowState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBoxWindowState.FormattingEnabled = true;
			this.ComboBoxWindowState.Items.AddRange(new object[] {
            "最大化",
            "通常"});
			this.ComboBoxWindowState.Location = new System.Drawing.Point(160, 360);
			this.ComboBoxWindowState.Name = "ComboBoxWindowState";
			this.ComboBoxWindowState.Size = new System.Drawing.Size(112, 20);
			this.ComboBoxWindowState.TabIndex = 11;
			this.ComboBoxWindowState.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWindowState_SelectedIndexChanged);
			// 
			// ButtonStop
			// 
			this.ButtonStop.BackgroundImage = global::YukkoView.Properties.Resources.ButtonStop;
			this.ButtonStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.ButtonStop.Location = new System.Drawing.Point(152, 44);
			this.ButtonStop.Name = "ButtonStop";
			this.ButtonStop.Size = new System.Drawing.Size(120, 28);
			this.ButtonStop.TabIndex = 3;
			this.ButtonStop.Text = "　　　　停止 (&P)";
			this.ButtonStop.UseVisualStyleBackColor = true;
			this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
			// 
			// ButtonStart
			// 
			this.ButtonStart.BackgroundImage = global::YukkoView.Properties.Resources.ButtonStart;
			this.ButtonStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.ButtonStart.Location = new System.Drawing.Point(16, 44);
			this.ButtonStart.Name = "ButtonStart";
			this.ButtonStart.Size = new System.Drawing.Size(120, 28);
			this.ButtonStart.TabIndex = 2;
			this.ButtonStart.Text = "　　　　開始 (&S)";
			this.ButtonStart.UseVisualStyleBackColor = true;
			this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
			// 
			// OpenFileDialogYukariConfigPath
			// 
			this.OpenFileDialogYukariConfigPath.Filter = "設定ファイル|*.ini|すべてのファイル|*.*";
			// 
			// FileSystemWatcherYukariConfig
			// 
			this.FileSystemWatcherYukariConfig.EnableRaisingEvents = true;
			this.FileSystemWatcherYukariConfig.SynchronizingObject = this;
			this.FileSystemWatcherYukariConfig.Changed += new System.IO.FileSystemEventHandler(this.FileSystemWatcherYukariConfig_Changed);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(0, 400);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(500, 5);
			this.panel1.TabIndex = 13;
			// 
			// ButtonSettings
			// 
			this.ButtonSettings.Location = new System.Drawing.Point(16, 420);
			this.ButtonSettings.Name = "ButtonSettings";
			this.ButtonSettings.Size = new System.Drawing.Size(96, 28);
			this.ButtonSettings.TabIndex = 14;
			this.ButtonSettings.Text = "環境設定 (&G)";
			this.ButtonSettings.UseVisualStyleBackColor = true;
			this.ButtonSettings.Click += new System.EventHandler(this.ButtonSettings_Click);
			// 
			// ButtonHelp
			// 
			this.ButtonHelp.Location = new System.Drawing.Point(280, 420);
			this.ButtonHelp.Name = "ButtonHelp";
			this.ButtonHelp.Size = new System.Drawing.Size(96, 28);
			this.ButtonHelp.TabIndex = 15;
			this.ButtonHelp.Text = "ヘルプ (&H)";
			this.ButtonHelp.UseVisualStyleBackColor = true;
			this.ButtonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
			// 
			// ContextMenuHelp
			// 
			this.ContextMenuHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ヘルプHToolStripMenuItem,
            this.toolStripSeparator1,
            this.改訂履歴UToolStripMenuItem,
            this.バージョン情報ToolStripMenuItem});
			this.ContextMenuHelp.Name = "ContextMenuHelp";
			this.ContextMenuHelp.Size = new System.Drawing.Size(162, 76);
			// 
			// ヘルプHToolStripMenuItem
			// 
			this.ヘルプHToolStripMenuItem.Name = "ヘルプHToolStripMenuItem";
			this.ヘルプHToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.ヘルプHToolStripMenuItem.Text = "ヘルプ (&H)";
			this.ヘルプHToolStripMenuItem.Click += new System.EventHandler(this.ヘルプHToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
			// 
			// 改訂履歴UToolStripMenuItem
			// 
			this.改訂履歴UToolStripMenuItem.Name = "改訂履歴UToolStripMenuItem";
			this.改訂履歴UToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.改訂履歴UToolStripMenuItem.Text = "改訂履歴 (&U)";
			this.改訂履歴UToolStripMenuItem.Click += new System.EventHandler(this.改訂履歴UToolStripMenuItem_Click);
			// 
			// バージョン情報ToolStripMenuItem
			// 
			this.バージョン情報ToolStripMenuItem.Name = "バージョン情報ToolStripMenuItem";
			this.バージョン情報ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.バージョン情報ToolStripMenuItem.Text = "バージョン情報 (&A)";
			this.バージョン情報ToolStripMenuItem.Click += new System.EventHandler(this.バージョン情報ToolStripMenuItem_Click);
			// 
			// ToolTipControl
			// 
			this.ToolTipControl.AutoPopDelay = 10000;
			this.ToolTipControl.InitialDelay = 500;
			this.ToolTipControl.ReshowDelay = 100;
			// 
			// FormControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(393, 464);
			this.Controls.Add(this.ButtonHelp);
			this.Controls.Add(this.ButtonSettings);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.ComboBoxWindowState);
			this.Controls.Add(this.ButtonMoveWindow);
			this.Controls.Add(this.ComboBoxDisplay);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.ButtonTest);
			this.Controls.Add(this.TextBoxTest);
			this.Controls.Add(this.ButtonStop);
			this.Controls.Add(this.ButtonStart);
			this.Controls.Add(this.LabelStatus);
			this.Controls.Add(this.LabelLamp);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormControl";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormControl_FormClosed);
			this.Load += new System.EventHandler(this.FormControl_Load);
			this.Shown += new System.EventHandler(this.FormControl_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcherYukariConfig)).EndInit();
			this.ContextMenuHelp.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label LabelLamp;
		private System.Windows.Forms.Label LabelStatus;
		private System.Windows.Forms.Button ButtonStart;
		private System.Windows.Forms.Button ButtonStop;
		private System.Windows.Forms.TextBox TextBoxTest;
		private System.Windows.Forms.Button ButtonTest;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton RadioButtonYukari;
		private System.Windows.Forms.RadioButton RadioButtonManual;
		private System.Windows.Forms.TextBox TextBoxRoomName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TextBoxServerUrl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox ComboBoxDisplay;
		private System.Windows.Forms.Button ButtonMoveWindow;
		private System.Windows.Forms.ComboBox ComboBoxWindowState;
		private System.Windows.Forms.Button ButtonBrowse;
		private System.Windows.Forms.TextBox TextBoxYukariConfigPath;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.OpenFileDialog OpenFileDialogYukariConfigPath;
		private System.IO.FileSystemWatcher FileSystemWatcherYukariConfig;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button ButtonSettings;
		private System.Windows.Forms.Button ButtonHelp;
		private System.Windows.Forms.ContextMenuStrip ContextMenuHelp;
		private System.Windows.Forms.ToolStripMenuItem ヘルプHToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 改訂履歴UToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem バージョン情報ToolStripMenuItem;
		private System.Windows.Forms.ToolTip ToolTipControl;
	}
}

