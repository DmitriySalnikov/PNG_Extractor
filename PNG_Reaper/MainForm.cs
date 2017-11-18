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

namespace PNG_Reaper
{
	public partial class PNG_Reaper : Form
	{
		int ArraySize = 50000;
		int MAX_ARRAY_SIZE = 100000000;
		int MIN_ARRAY_SIZE = 100;

		byte[] PNG_Start = new byte[] { 0x89, 0x50, 0x4e, 0x47 };
		byte[] PNG_End = new byte[] { 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82 };

		public PNG_Reaper()
		{
			InitializeComponent();
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

			String path = openFileDialog1.FileName;

			if (path != "" && !File.Exists(path))
			{
				return;
			}
			String file_name = path.Split('\\').Last();
			String folder = path.Replace(file_name, "") + "PNGs_from_" + file_name + "\\";

			BinaryReader reader = new BinaryReader(File.OpenRead(path));
			Stream stream = reader.BaseStream;
			BinaryWriter writer;

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

				while (offset_start != -1 || offset_end != -1)
				{
					int pos;
					if (offset_start != -1)
					{
						pos = find_array(ref arr, ref PNG_Start, offset_start);
						offset_start = pos;
						if (pos != -1)
						{
							offset_start += 4; //PREVENTING INFINITE LOOP

							long new_position = old_pos + pos;
							if (!starts.Contains(new_position))
								starts.Add(new_position);
						}
					}

					if (offset_end != -1)
					{
						pos = find_array(ref arr, ref PNG_End, offset_end, true);
						offset_end = pos;
						if (pos != -1)
						{
							long new_position = old_pos + pos;
							if (!ends.Contains(new_position))
								ends.Add(new_position);
						}
					}
				}

				if (size > 8)
				{
					stream.Seek(-8, SeekOrigin.Current);
				}
			}

			int first = starts.Count;
			int second = ends.Count;
			if (second >= first)
				second = first; //for get something if part of data corrupted
			else
				first = 0;

			if (first > 0)
			{
				for (int i = 0; i < first; i++)
				{
					Directory.CreateDirectory(folder);
					String new_file_name = folder + i.ToString() + ".png";
					if (File.Exists(new_file_name))
						File.Delete(new_file_name);

					writer = new BinaryWriter(File.OpenWrite(folder + i.ToString() + ".png"));

					stream.Seek(starts[i], SeekOrigin.Begin);
					long size = ends[i] - starts[i];

					while (size > 0)
					{
						if (size > int.MaxValue)
						{
							size -= int.MaxValue;
							writer.Write(reader.ReadBytes(int.MaxValue));
						}
						else
						{
							writer.Write(reader.ReadBytes((int)size));
							size = 0;
						}
					}
					writer.Close();
				}
				MessageBox.Show("Exported PNGs: " + first.ToString(), "Done!");
				reader.Close();
			}
			else
				MessageBox.Show("Nothing found!", "Done!");

			reader.Close();
		}

		private void PNG_Reaper_Load(object sender, EventArgs e)
		{
			tb_chunck.Text = ArraySize.ToString();
		}

		private void parce_chunk_value()
		{
			Int32.TryParse(tb_chunck.Text, out int res);
			ArraySize = res;
			if (res > MAX_ARRAY_SIZE)
				res = MAX_ARRAY_SIZE;
			if (res < MIN_ARRAY_SIZE)
				res = MIN_ARRAY_SIZE;

			tb_chunck.Text = res.ToString();
		}
	}
}
