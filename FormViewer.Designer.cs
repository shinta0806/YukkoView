namespace YukkoView
{
	partial class FormViewer
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
			this.components = new System.ComponentModel.Container();
			this.TimerPlay = new System.Windows.Forms.Timer(this.components);
			this.ButtonDisplay = new System.Windows.Forms.Button();
			this.LabelGrip = new System.Windows.Forms.Label();
			this.ButtonMaximize = new System.Windows.Forms.Button();
			this.ToolTipViewer = new System.Windows.Forms.ToolTip(this.components);
			this.TimerTopMost = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// TimerPlay
			// 
			this.TimerPlay.Interval = 50;
			this.TimerPlay.Tick += new System.EventHandler(this.TimerPlay_Tick);
			// 
			// ButtonDisplay
			// 
			this.ButtonDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonDisplay.Location = new System.Drawing.Point(400, 0);
			this.ButtonDisplay.Name = "ButtonDisplay";
			this.ButtonDisplay.Size = new System.Drawing.Size(48, 24);
			this.ButtonDisplay.TabIndex = 1;
			this.ButtonDisplay.Text = "v";
			this.ToolTipViewer.SetToolTip(this.ButtonDisplay, "ディスプレイ切り替え");
			this.ButtonDisplay.UseVisualStyleBackColor = true;
			this.ButtonDisplay.Click += new System.EventHandler(this.ButtonDisplay_Click);
			// 
			// LabelGrip
			// 
			this.LabelGrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelGrip.BackColor = System.Drawing.Color.Gainsboro;
			this.LabelGrip.Location = new System.Drawing.Point(504, 316);
			this.LabelGrip.Name = "LabelGrip";
			this.LabelGrip.Size = new System.Drawing.Size(24, 24);
			this.LabelGrip.TabIndex = 2;
			this.LabelGrip.Text = "o";
			this.LabelGrip.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.LabelGrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormViewer_MouseDown);
			this.LabelGrip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LabelGrip_MouseMove);
			this.LabelGrip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormViewer_MouseUp);
			// 
			// ButtonMaximize
			// 
			this.ButtonMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonMaximize.Location = new System.Drawing.Point(448, 0);
			this.ButtonMaximize.Name = "ButtonMaximize";
			this.ButtonMaximize.Size = new System.Drawing.Size(48, 24);
			this.ButtonMaximize.TabIndex = 3;
			this.ButtonMaximize.UseVisualStyleBackColor = true;
			this.ButtonMaximize.Click += new System.EventHandler(this.ButtonMaximize_Click);
			// 
			// TimerTopMost
			// 
			this.TimerTopMost.Interval = 1000;
			this.TimerTopMost.Tick += new System.EventHandler(this.TimerTopMost_Tick);
			// 
			// FormViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(528, 340);
			this.Controls.Add(this.ButtonMaximize);
			this.Controls.Add(this.LabelGrip);
			this.Controls.Add(this.ButtonDisplay);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MinimumSize = new System.Drawing.Size(200, 100);
			this.Name = "FormViewer";
			this.ShowInTaskbar = false;
			this.Text = "FormViewer";
			this.TopMost = true;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormViewer_FormClosed);
			this.Load += new System.EventHandler(this.FormViewer_Load);
			this.Shown += new System.EventHandler(this.FormViewer_Shown);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormViewer_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormViewer_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormViewer_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormViewer_MouseUp);
			this.Resize += new System.EventHandler(this.FormViewer_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer TimerPlay;
		private System.Windows.Forms.Button ButtonDisplay;
		private System.Windows.Forms.Label LabelGrip;
		private System.Windows.Forms.Button ButtonMaximize;
		private System.Windows.Forms.ToolTip ToolTipViewer;
		private System.Windows.Forms.Timer TimerTopMost;
	}
}