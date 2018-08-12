namespace YukkoView
{
	partial class FormSettings
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
			this.CheckBoxAutoRun = new System.Windows.Forms.CheckBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.CheckBoxEnableMargin = new System.Windows.Forms.CheckBox();
			this.TextBoxMarginPercent = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.ButtonLog = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.ButtonCheckRss = new System.Windows.Forms.Button();
			this.ProgressBarCheckRss = new System.Windows.Forms.ProgressBar();
			this.CheckBoxCheckRss = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.SaveFileDialogLog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// CheckBoxAutoRun
			// 
			this.CheckBoxAutoRun.AutoSize = true;
			this.CheckBoxAutoRun.Location = new System.Drawing.Point(16, 48);
			this.CheckBoxAutoRun.Name = "CheckBoxAutoRun";
			this.CheckBoxAutoRun.Size = new System.Drawing.Size(198, 16);
			this.CheckBoxAutoRun.TabIndex = 2;
			this.CheckBoxAutoRun.Text = "起動と同時にコメント表示を開始する";
			this.CheckBoxAutoRun.UseVisualStyleBackColor = true;
			// 
			// panel5
			// 
			this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel5.Location = new System.Drawing.Point(0, 20);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(500, 5);
			this.panel5.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "　環境設定　";
			// 
			// CheckBoxEnableMargin
			// 
			this.CheckBoxEnableMargin.AutoSize = true;
			this.CheckBoxEnableMargin.Location = new System.Drawing.Point(16, 80);
			this.CheckBoxEnableMargin.Name = "CheckBoxEnableMargin";
			this.CheckBoxEnableMargin.Size = new System.Drawing.Size(136, 16);
			this.CheckBoxEnableMargin.TabIndex = 3;
			this.CheckBoxEnableMargin.Text = "ビューアウィンドウの上下";
			this.CheckBoxEnableMargin.UseVisualStyleBackColor = true;
			this.CheckBoxEnableMargin.CheckedChanged += new System.EventHandler(this.CheckBoxEnableMargin_CheckedChanged);
			// 
			// TextBoxMarginPercent
			// 
			this.TextBoxMarginPercent.Location = new System.Drawing.Point(160, 76);
			this.TextBoxMarginPercent.Name = "TextBoxMarginPercent";
			this.TextBoxMarginPercent.Size = new System.Drawing.Size(56, 19);
			this.TextBoxMarginPercent.TabIndex = 4;
			this.TextBoxMarginPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(232, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(137, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "[%] にはコメントを表示しない";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(0, 116);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(500, 5);
			this.panel1.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 12);
			this.label1.TabIndex = 7;
			this.label1.Text = "　メンテナンス　";
			// 
			// ButtonLog
			// 
			this.ButtonLog.Location = new System.Drawing.Point(184, 232);
			this.ButtonLog.Name = "ButtonLog";
			this.ButtonLog.Size = new System.Drawing.Size(216, 28);
			this.ButtonLog.TabIndex = 12;
			this.ButtonLog.Text = "ログ保存(&X)";
			this.ButtonLog.UseVisualStyleBackColor = true;
			this.ButtonLog.Click += new System.EventHandler(this.ButtonLog_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 208);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(376, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "ログを保存する";
			// 
			// ButtonCheckRss
			// 
			this.ButtonCheckRss.Location = new System.Drawing.Point(184, 164);
			this.ButtonCheckRss.Name = "ButtonCheckRss";
			this.ButtonCheckRss.Size = new System.Drawing.Size(216, 28);
			this.ButtonCheckRss.TabIndex = 10;
			this.ButtonCheckRss.Text = "今すぐ最新情報を確認する (&A)";
			this.ButtonCheckRss.UseVisualStyleBackColor = true;
			this.ButtonCheckRss.Click += new System.EventHandler(this.ButtonCheckRss_Click);
			// 
			// ProgressBarCheckRss
			// 
			this.ProgressBarCheckRss.Location = new System.Drawing.Point(16, 164);
			this.ProgressBarCheckRss.MarqueeAnimationSpeed = 10;
			this.ProgressBarCheckRss.Name = "ProgressBarCheckRss";
			this.ProgressBarCheckRss.Size = new System.Drawing.Size(152, 28);
			this.ProgressBarCheckRss.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.ProgressBarCheckRss.TabIndex = 9;
			this.ProgressBarCheckRss.Visible = false;
			// 
			// CheckBoxCheckRss
			// 
			this.CheckBoxCheckRss.Location = new System.Drawing.Point(16, 140);
			this.CheckBoxCheckRss.Name = "CheckBoxCheckRss";
			this.CheckBoxCheckRss.Size = new System.Drawing.Size(384, 16);
			this.CheckBoxCheckRss.TabIndex = 8;
			this.CheckBoxCheckRss.Text = "ゆっこビューの最新情報・更新版を自動的に確認する (&L)";
			this.CheckBoxCheckRss.UseVisualStyleBackColor = true;
			this.CheckBoxCheckRss.CheckedChanged += new System.EventHandler(this.CheckBoxCheckRss_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Location = new System.Drawing.Point(0, 276);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(500, 5);
			this.panel2.TabIndex = 13;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(304, 296);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(96, 28);
			this.ButtonCancel.TabIndex = 15;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(184, 296);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(96, 28);
			this.ButtonOK.TabIndex = 14;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// SaveFileDialogLog
			// 
			this.SaveFileDialogLog.Filter = "ログファイル|*.lga";
			// 
			// FormSettings
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(414, 338);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.ButtonLog);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.ButtonCheckRss);
			this.Controls.Add(this.ProgressBarCheckRss);
			this.Controls.Add(this.CheckBoxCheckRss);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TextBoxMarginPercent);
			this.Controls.Add(this.CheckBoxEnableMargin);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.CheckBoxAutoRun);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSettings";
			this.Load += new System.EventHandler(this.FormSettings_Load);
			this.Shown += new System.EventHandler(this.FormSettings_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.CheckBox CheckBoxAutoRun;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox CheckBoxEnableMargin;
		private System.Windows.Forms.TextBox TextBoxMarginPercent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ButtonLog;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button ButtonCheckRss;
		private System.Windows.Forms.ProgressBar ProgressBarCheckRss;
		private System.Windows.Forms.CheckBox CheckBoxCheckRss;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.SaveFileDialog SaveFileDialogLog;
	}
}