using PNG_Extractor.Extrators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNG_Extractor
{
	public partial class PNG_Extractor : Form
	{
		public const int BufferSize = 64 * 1024;
		bool is_closing = false;

		public PNG_Extractor()
		{
			InitializeComponent();

			Icon = Properties.Resources.smallicon;
		}

		private void b_select_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				l_file.Text = openFileDialog1.FileName;
			}
		}

		private void b_reap_Click(object sender, EventArgs e)
		{
			string path = l_file.Text;

			if (path != "" && !File.Exists(path))
			{
				MessageBox.Show($"File \"{path}\" not found!", "Error!");
				return;
			}

			string file_name = Path.GetFileName(path);
			string folder = path.Replace(file_name, "") + "Images_from_" + file_name + "\\";


			var extractor = new PNGExtractor();
			ExtractorResult res = new ExtractorResult() { ExtractorName = extractor.Name, IsSuccess = false, ExportedCount = -1, FoundCount = -1 };

			DoWorkEventHandler dwEh = ((se, ev) =>
			{
				try
				{
					BinaryReader reader = new BinaryReader(File.OpenRead(path));
					res = extractor.Extract(reader, folder, bg_worker);
					reader.Close();
				}
				catch (Exception ex)
				{
					if (!is_closing)
						MessageBox.Show(ex.Message, "Error");
				}
				return;
			});

			bg_worker.DoWork += dwEh;
			bg_worker.RunWorkerAsync();
			var epf = new ExtractProgressForm(bg_worker);
			epf.ShowDialog();
			bg_worker.DoWork -= dwEh;

			if (res.IsSuccess)
			{
				MessageBox.Show($"{res.ExtractorName}: \nFound: {res.FoundCount}\nExtracted: {res.ExportedCount}", "Done!");
			}
			else
			{
				if (res.ExportedCount == -1 && res.FoundCount == -1)
				{
					MessageBox.Show($"{res.ExtractorName}: \nError or Cancelled!", "Done!");
				}
				else
				{
					MessageBox.Show($"{res.ExtractorName}: \nNothing found!", "Done!");
				}
			}

		}

		private void PNG_Extractor_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Effect == DragDropEffects.Move)
			{
				var d = (string[])e.Data.GetData(DataFormats.FileDrop);
				l_file.Text = d[0];
			}
		}

		private void PNG_Extractor_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var d = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (d.Length == 1 && File.Exists(d[0]))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private void PNG_Reaper_Load(object sender, EventArgs e)
		{

		}

		private void PNG_Extractor_FormClosing(object sender, FormClosingEventArgs e)
		{
			is_closing = true;
		}

		private void PNG_Extractor_Shown(object sender, EventArgs e)
		{
			is_closing = false;
		}
	}
}
