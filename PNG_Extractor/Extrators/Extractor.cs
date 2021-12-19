using System.Collections.Generic;
using System.IO;

namespace PNG_Extractor.Extrators
{
    public struct BGWorkerInitProgress
    {
        public string ExtractorName;
    }

    public struct BGWorkerProgress
    {
        public string Text;
    }

    public enum SaveExtractedFileError
    {
        OK,
        CantOpenToWrite,
        ZeroSize,
    }

    public struct ExtractorResult
    {
        public string ExtractorName;
        public bool IsSuccess;
        public bool IsCancelledOrError;
        public int ExportedCount;
        public int FoundCount;
    }

    public class ExtractorFile
    {
        public long StartPos;
        public long Size;
        public BinaryReader Stream;

        public virtual SaveExtractedFileError Save(string name)
        {
            Stream.BaseStream.Seek(StartPos, SeekOrigin.Begin);
            long size = Size;

            string new_file_name = name;
            if (File.Exists(new_file_name))
                File.Delete(new_file_name);

            BinaryWriter writer;
            if (size > 0)
            {
                try
                {
                    writer = new BinaryWriter(File.OpenWrite(name));
                }
                catch
                {
                    return SaveExtractedFileError.CantOpenToWrite;
                }
            }
            else
            {
                return SaveExtractedFileError.ZeroSize;
            }

            while (size > 0)
            {
                if (size > PNG_Extractor.BufferSize)
                {
                    size -= PNG_Extractor.BufferSize;
                    writer.Write(Stream.ReadBytes(PNG_Extractor.BufferSize));
                }
                else
                {
                    writer.Write(Stream.ReadBytes((int)size));
                    size = 0;
                }
            }

            writer?.Close();

            return SaveExtractedFileError.OK;
        }
    }

    public class Extractor
    {
        protected List<ExtractorFile> Files;

        public virtual string Name { get; }
        private BackgroundWorkerCustom bg_worker = null;
        public BackgroundWorkerCustom BGWorker
        {
            get => bg_worker;
            set
            {
                bg_worker = value;
            }
        }

        protected bool IsCancelled
        {
            get => bg_worker != null && bg_worker.CancellationPending;
        }

        protected bool IsStopScanning
        {
            get => bg_worker != null && bg_worker.IsStopPartOfTask;
        }

        public Extractor()
        {

        }

        public Extractor(BackgroundWorkerCustom bgw)
        {
            BGWorker = bgw;
        }

        protected void ReportInit()
        {
            bg_worker?.ReportProgress(0, new BGWorkerInitProgress() { ExtractorName = Name });
        }

        protected void ReportProgress(int progress, string text = "")
        {
            bg_worker?.ReportProgress(progress, new BGWorkerProgress() { Text = text });
        }

        public virtual ExtractorResult Extract(BinaryReader stream, string extract_directory)
        {
            return new ExtractorResult();
        }
    }
}
