namespace PNG_Extractor
{
	partial class PNG_Extractor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PNG_Extractor));
			this.b_reap = new System.Windows.Forms.Button();
			this.l_file = new System.Windows.Forms.Label();
			this.b_select = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.nud_chunk = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.nud_chunk)).BeginInit();
			this.SuspendLayout();
			// 
			// b_reap
			// 
			resources.ApplyResources(this.b_reap, "b_reap");
			this.b_reap.Name = "b_reap";
			this.b_reap.UseVisualStyleBackColor = true;
			this.b_reap.Click += new System.EventHandler(this.b_reap_Click);
			// 
			// l_file
			// 
			resources.ApplyResources(this.l_file, "l_file");
			this.l_file.Name = "l_file";
			// 
			// b_select
			// 
			resources.ApplyResources(this.b_select, "b_select");
			this.b_select.Name = "b_select";
			this.b_select.UseVisualStyleBackColor = true;
			this.b_select.Click += new System.EventHandler(this.b_select_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "somefile";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// toolTip1
			// 
			this.toolTip1.ToolTipTitle = "Размер блока считываемых данных. Может повлиять на производительность.";
			// 
			// nud_chunk
			// 
			resources.ApplyResources(this.nud_chunk, "nud_chunk");
			this.nud_chunk.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
			this.nud_chunk.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nud_chunk.Name = "nud_chunk";
			this.nud_chunk.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// PNG_Extractor
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nud_chunk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.b_select);
			this.Controls.Add(this.l_file);
			this.Controls.Add(this.b_reap);
			this.Name = "PNG_Extractor";
			this.Load += new System.EventHandler(this.PNG_Reaper_Load);
			((System.ComponentModel.ISupportInitialize)(this.nud_chunk)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button b_reap;
		private System.Windows.Forms.Label l_file;
		private System.Windows.Forms.Button b_select;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.NumericUpDown nud_chunk;
	}
}

