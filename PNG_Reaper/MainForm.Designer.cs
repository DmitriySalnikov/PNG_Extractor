namespace PNG_Reaper
{
	partial class PNG_Reaper
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PNG_Reaper));
			this.b_reap = new System.Windows.Forms.Button();
			this.l_file = new System.Windows.Forms.Label();
			this.b_select = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.tb_chunck = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// b_reap
			// 
			resources.ApplyResources(this.b_reap, "b_reap");
			this.b_reap.Name = "b_reap";
			this.toolTip1.SetToolTip(this.b_reap, resources.GetString("b_reap.ToolTip"));
			this.b_reap.UseVisualStyleBackColor = true;
			this.b_reap.Click += new System.EventHandler(this.b_reap_Click);
			// 
			// l_file
			// 
			resources.ApplyResources(this.l_file, "l_file");
			this.l_file.Name = "l_file";
			this.toolTip1.SetToolTip(this.l_file, resources.GetString("l_file.ToolTip"));
			// 
			// b_select
			// 
			resources.ApplyResources(this.b_select, "b_select");
			this.b_select.Name = "b_select";
			this.toolTip1.SetToolTip(this.b_select, resources.GetString("b_select.ToolTip"));
			this.b_select.UseVisualStyleBackColor = true;
			this.b_select.Click += new System.EventHandler(this.b_select_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "somefile";
			resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			// 
			// tb_chunck
			// 
			resources.ApplyResources(this.tb_chunck, "tb_chunck");
			this.tb_chunck.Name = "tb_chunck";
			this.toolTip1.SetToolTip(this.tb_chunck, resources.GetString("tb_chunck.ToolTip"));
			// 
			// toolTip1
			// 
			this.toolTip1.ToolTipTitle = "Размер блока считываемых данных. Может повлиять на производительность.";
			// 
			// PNG_Reaper
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tb_chunck);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.b_select);
			this.Controls.Add(this.l_file);
			this.Controls.Add(this.b_reap);
			this.Name = "PNG_Reaper";
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.Load += new System.EventHandler(this.PNG_Reaper_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button b_reap;
		private System.Windows.Forms.Label l_file;
		private System.Windows.Forms.Button b_select;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tb_chunck;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}

