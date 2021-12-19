using System.ComponentModel;

namespace PNG_Extractor
{
    public class BackgroundWorkerCustom : BackgroundWorker
    {
        private bool is_stop_part_of_task = false;
        public bool IsStopPartOfTask
        {
            get => is_stop_part_of_task;
        }

        public void StopPartOfWork()
        {
            is_stop_part_of_task = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            is_stop_part_of_task = false;
            base.OnDoWork(e);
        }
    }
}
