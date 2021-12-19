using System;
using System.Windows.Forms;

namespace PNG_Extractor
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
                var ext = new PNG_Extractor();
                ext.StartExtract(args[1]);
                return;
            }

            Application.Run(new PNG_Extractor());
        }
    }
}
