using Image_Extractor.Extrators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Image_Extractor
{
    public partial class Image_Extractor_Main_Form : Form
    {
        public const int BufferSize = 64 * 1024;
        bool is_closing = false;

        public Image_Extractor_Main_Form()
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
            StartExtract(l_file.Text);
        }

        public void StartExtract(string path)
        {
            if (path != "" && !File.Exists(path))
            {
                MessageBox.Show($"File \"{path}\" not found!", "Error!");
                return;
            }

            string file_name = Path.GetFileName(path);
            string folder = path.Replace(file_name, "") + "Images_from_" + file_name + "\\";

            DoWorkEventHandler dwEh = ((se, ev) =>
            {
                try
                {
                    List<ExtractorFile> foundImagesList = new List<ExtractorFile>();

                    using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
                    {
                        foreach (var extractor in new Extractor[] { new PNGExtractor(bg_worker), new WEBPExtractor(bg_worker) })
                        {
                            reader.BaseStream.Seek(0, SeekOrigin.Begin);

                            ExtractorResult res = new ExtractorResult() { ExtractorName = extractor.Name, IsSuccess = false };
                            res = extractor.Extract(reader, folder);

                            if (res.IsSuccess)
                            {
                                foundImagesList.AddRange(res.Files);
                            }
                            else
                            {
                                if (res.IsCancelledOrError)
                                {
                                    MessageBox.Show($"{res.ExtractorName}: \nCancelled or Error occurred!", "Error!");
                                    return;
                                }
                            }
                        }

                        int exportedCount = 0;

                        bg_worker?.ReportProgress(0, new BGWorkerInitProgress() { ExtractorName = "Extracting Images" });

                        if (foundImagesList.Count > 0)
                            Directory.CreateDirectory(folder);

                        for (int idx = 0; idx < foundImagesList.Count; idx++)
                        {
                            if (bg_worker.CancellationPending)
                                break;

                            bg_worker?.ReportProgress((int)(100.0 * exportedCount / foundImagesList.Count), new BGWorkerProgress() { Text = $"Current: {idx}, Saved files: {exportedCount}, Total files: {foundImagesList.Count}" });

                            var i = foundImagesList[idx];
                            if (i.Save(Path.Combine(folder, $"{idx}{i.Extension}")) == SaveExtractedFileError.OK)
                                exportedCount++;
                        };

                        if (foundImagesList.Count == 0)
                        {
                            MessageBox.Show($"Nothing found!", "Done!");
                        }
                        else
                        {
                            MessageBox.Show($"Found: {foundImagesList.Count}\nExtracted: {exportedCount}", "Done!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!is_closing)
                        MessageBox.Show(ex.Message, "Error");
                }
            });

            bg_worker.DoWork += dwEh;
            bg_worker.RunWorkerAsync();
            var epf = new ExtractProgressForm(bg_worker);
            epf.ShowDialog();
            bg_worker.DoWork -= dwEh;

        }

        private void Image_Extractor_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                var d = (string[])e.Data.GetData(DataFormats.FileDrop);
                l_file.Text = d[0];
            }
        }

        private void Image_Extractor_DragEnter(object sender, DragEventArgs e)
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

        private void Image_Extractor_FormClosing(object sender, FormClosingEventArgs e)
        {
            is_closing = true;
        }

        private void Image_Extractor_Shown(object sender, EventArgs e)
        {
            is_closing = false;
        }
    }
}
