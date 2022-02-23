using Image_Extractor.Extrators;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Image_Extractor
{
    public partial class ExtractProgressForm : Form
    {
        BackgroundWorkerCustom bg_worker = null;
        bool is_closing_by_event = false;
        DateTime prevChangeTime = DateTime.Now;

        public ExtractProgressForm()
        {
            InitializeComponent();
            is_closing_by_event = false;
            btn_stop_scan.Enabled = true;
        }

        public ExtractProgressForm(BackgroundWorkerCustom bgw)
        {
            InitializeComponent();
            is_closing_by_event = false;
            btn_stop_scan.Enabled = true;

            bg_worker = bgw;

            bgw.ProgressChanged += Bgw_ProgressChanged;
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_stop_scan.Enabled = true;
            is_closing_by_event = true;
            Close();
        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            btn_stop_scan.Enabled = !bg_worker.IsStopPartOfTask;
            pb_progress.Value = e.ProgressPercentage;

            if (e.UserState is BGWorkerInitProgress bgInitData)
                l_name.Text = bgInitData.ExtractorName;

            if (e.UserState is BGWorkerProgress bgProgData)
            {
                if ((DateTime.Now - prevChangeTime).TotalSeconds > 0.25)
                {
                    prevChangeTime = DateTime.Now;
                    l_add_info.Text = bgProgData.Text;
                }
            }
        }

        private void ExtractProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_closing_by_event)
                bg_worker.CancelAsync();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            bg_worker.CancelAsync();
        }

        private void btn_stop_scan_Click(object sender, EventArgs e)
        {
            bg_worker.StopPartOfWork();
            btn_stop_scan.Enabled = false;
        }
    }
}
