using System.Collections.Generic;
using System.IO;

namespace Image_Extractor.Extrators
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
        public List<ExtractorFile> Files;
    }

    public class ExtractorFile
    {
        public long StartPos;
        public long Size;
        public string Extension;
        public BinaryReader Stream;

        public virtual SaveExtractedFileError Save(string name)
        {
            Stream.BaseStream.Seek(StartPos, SeekOrigin.Begin);
            long size = Size;

            if (File.Exists(name))
                try
                {
                    File.Delete(name);
                }
                catch
                {
                    return SaveExtractedFileError.CantOpenToWrite;
                }

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
                if (size > Image_Extractor_Main_Form.BufferSize)
                {
                    size -= Image_Extractor_Main_Form.BufferSize;
                    writer.Write(Stream.ReadBytes(Image_Extractor_Main_Form.BufferSize));
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
