using System;
using System.Windows.Forms;

namespace Image_Extractor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var ext = new Image_Extractor_Main_Form();
                ext.StartExtract(args[1]);
                return;
            }

            Application.Run(new Image_Extractor_Main_Form());
        }
    }
}
