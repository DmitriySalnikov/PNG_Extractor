using PNG_Extractor.Extrators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNG_Extractor
{
	public partial class ExtractProgressForm : Form
	{
		BackgroundWorker bg_worker = null;
		bool is_closing_by_event = false;

		public ExtractProgressForm()
		{
			InitializeComponent();
		}

		public ExtractProgressForm(BackgroundWorker bgw)
		{
			InitializeComponent();

			bg_worker = bgw;

			bgw.ProgressChanged += Bgw_ProgressChanged;
			bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
		}

		private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Close();
		}

		private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			pb_progress.Value = e.ProgressPercentage;

			if (e.UserState is BGWorkerInitProgress bgInitData)
				l_name.Text = bgInitData.ExtractorName;

			if (e.UserState is BGWorkerProgress bgProgData)
				l_add_info.Text = bgProgData.Text;
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
	}
}
