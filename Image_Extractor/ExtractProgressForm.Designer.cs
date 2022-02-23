namespace Image_Extractor
{
	partial class ExtractProgressForm
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
			this.pb_progress = new System.Windows.Forms.ProgressBar();
			this.l_name = new System.Windows.Forms.Label();
			this.l_add_info = new System.Windows.Forms.Label();
			this.btn_cancel = new System.Windows.Forms.Button();
			this.btn_stop_scan = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pb_progress
			// 
			this.pb_progress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_progress.Location = new System.Drawing.Point(12, 25);
			this.pb_progress.Name = "pb_progress";
			this.pb_progress.Size = new System.Drawing.Size(503, 23);
			this.pb_progress.TabIndex = 0;
			// 
			// l_name
			// 
			this.l_name.AutoSize = true;
			this.l_name.Location = new System.Drawing.Point(9, 9);
			this.l_name.Name = "l_name";
			this.l_name.Size = new System.Drawing.Size(43, 13);
			this.l_name.TabIndex = 1;
			this.l_name.Text = "Waiting";
			// 
			// l_add_info
			// 
			this.l_add_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.l_add_info.AutoSize = true;
			this.l_add_info.Location = new System.Drawing.Point(12, 59);
			this.l_add_info.Name = "l_add_info";
			this.l_add_info.Size = new System.Drawing.Size(16, 13);
			this.l_add_info.TabIndex = 2;
			this.l_add_info.Text = "...";
			// 
			// btn_cancel
			// 
			this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cancel.Location = new System.Drawing.Point(440, 54);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_cancel.TabIndex = 3;
			this.btn_cancel.Text = "Cancel";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
			// 
			// btn_stop_scan
			// 
			this.btn_stop_scan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_stop_scan.Location = new System.Drawing.Point(359, 54);
			this.btn_stop_scan.Name = "btn_stop_scan";
			this.btn_stop_scan.Size = new System.Drawing.Size(75, 23);
			this.btn_stop_scan.TabIndex = 4;
			this.btn_stop_scan.Text = "Stop Scan";
			this.btn_stop_scan.UseVisualStyleBackColor = true;
			this.btn_stop_scan.Click += new System.EventHandler(this.btn_stop_scan_Click);
			// 
			// ExtractProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 85);
			this.Controls.Add(this.btn_stop_scan);
			this.Controls.Add(this.btn_cancel);
			this.Controls.Add(this.l_add_info);
			this.Controls.Add(this.l_name);
			this.Controls.Add(this.pb_progress);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1920, 124);
			this.MinimumSize = new System.Drawing.Size(434, 124);
			this.Name = "ExtractProgressForm";
			this.Text = "ExtractProgressForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExtractProgressForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pb_progress;
		private System.Windows.Forms.Label l_name;
		private System.Windows.Forms.Label l_add_info;
		private System.Windows.Forms.Button btn_cancel;
		private System.Windows.Forms.Button btn_stop_scan;
	}
}