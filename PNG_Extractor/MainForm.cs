using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNG_Extractor
{
	public partial class PNG_Extractor : Form
	{
		int ArraySize = 64 * 1024;

		byte[] PNG_Start = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
		byte[] PNG_End = new byte[] { 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82 };
		bool is_start_found = false;

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

		private int find_array(ref byte[] arr, ref byte[] template, int offset, bool for_end = false)
		{
			bool found = false;
			int res = -1;
			if (arr.Length >= template.Length)
			{
				for (int i = offset; i <= arr.Length - template.Length; i++)
				{
					if (arr[i] == template[0])
					{
						for (int j = 1; j < template.Length; j++)
						{
							if (arr[i + j] != template[j])
								break;
							if (j == template.Length - 1)
							{
								if (!for_end)
									res = i;
								else
									res = i + j + 1;
								found = true;
								break;
							}
						}
					}
					if (found)
						break;
				}
			}
			return res;
		}

		private void b_reap_Click(object sender, EventArgs e)
		{
			parce_chunk_value();

			string path = l_file.Text;

			if (path != "" && !File.Exists(path))
			{
				return;
			}
			string file_name = Path.GetFileName(path);
			string folder = path.Replace(file_name, "") + "PNGs_from_" + file_name + "\\";

			BinaryReader reader = new BinaryReader(File.OpenRead(path));
			Stream stream = reader.BaseStream;

			List<long> starts = new List<long>();
			List<long> ends = new List<long>();

			while (stream.Position < stream.Length)
			{

				long to_end = stream.Length - stream.Position;
				long old_pos = stream.Position;
				int size = to_end < ArraySize ? (int)to_end : (ArraySize > stream.Length) ? (int)stream.Length : ArraySize;

				byte[] arr = reader.ReadBytes(size);
				int offset_start = 0;
				int offset_end = 0;

				while (is_start_found ? offset_end != -1 : offset_start != -1)
				{
					int pos;
					if (!is_start_found && offset_start != -1)
					{
						pos = find_array(ref arr, ref PNG_Start, offset_start);
						offset_start = pos;
						if (pos != -1)
						{
							offset_start += 4; //PREVENTING INFINITE LOOP

							long new_position = old_pos + pos;
							if (!starts.Contains(new_position))
							{
								starts.Add(new_position);
								is_start_found = true;
							}
						}
					}

					if (is_start_found && offset_end != -1)
					{
						pos = find_array(ref arr, ref PNG_End, offset_end, true);
						offset_end = pos;
						if (pos != -1)
						{
							long new_position = old_pos + pos;
							if ((starts.Count < 2 || new_position > starts[starts.Count - 2]) && !ends.Contains(new_position))
							{
								ends.Add(new_position);
								is_start_found = false;
							}
						}
					}
				}

				if (size > PNG_Start.Length)
				{
					stream.Seek(-(PNG_Start.Length - 1), SeekOrigin.Current);
				}
			}
			is_start_found = false;

			int first = starts.Count;
			int second = ends.Count;
			if (second >= first)
				second = first; //for get something if part of data corrupted
			else
				first = second;

			int exported_count = 0;
			if (first > 0)
			{
				Directory.CreateDirectory(folder);
				for (int i = 0; i < first; i++)
				{
					string new_file_name = folder + i.ToString() + ".png";
					if (File.Exists(new_file_name))
						File.Delete(new_file_name);

					stream.Seek(starts[i], SeekOrigin.Begin);
					long size = ends[i] - starts[i];

					BinaryWriter writer = null;
					if (size > 0)
					{
						writer = new BinaryWriter(File.OpenWrite(folder + i.ToString() + ".png"));
						exported_count++;
					}

					while (size > 0)
					{
						if (size > ArraySize)
						{
							size -= ArraySize;
							writer.Write(reader.ReadBytes(ArraySize));
						}
						else
						{
							writer.Write(reader.ReadBytes((int)size));
							size = 0;
						}
					}

					writer?.Close();
				}
				MessageBox.Show("Exported PNGs: " + exported_count, "Done!");
				reader.Close();
			}
			else
				MessageBox.Show("Nothing found!", "Done!");

			reader.Close();
		}

		private void PNG_Reaper_Load(object sender, EventArgs e)
		{

		}

		private void parce_chunk_value()
		{

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

		private void PNG_Extractor_DragLeave(object sender, EventArgs e)
		{

		}
	}
}
