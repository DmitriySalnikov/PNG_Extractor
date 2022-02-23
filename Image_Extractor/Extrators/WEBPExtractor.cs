using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Image_Extractor.Extrators
{
    public class WEBPExtractor : Extractor
    {
        byte[] RIFF_Start = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        byte[] WEBP_Start = new byte[] { 0x57, 0x45, 0x42, 0x50 };

        public WEBPExtractor()
        {
        }

        public WEBPExtractor(BackgroundWorkerCustom bgw) : base(bgw)
        {
        }

        public override string Name => "WEBP Extractor";

        public override ExtractorResult Extract(BinaryReader stream, string extract_directory)
        {
            ReportInit();
            var res = new ExtractorResult() { ExtractorName = Name, IsSuccess = false, IsCancelledOrError = true };

            Files = new List<ExtractorFile>();

            // Scanning of WEBPs in whole file
            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                //max_position = Math.Max(stream.BaseStream.Position, max_position);

                if (IsCancelled) return res;
                if (IsStopScanning) break;

                ReportProgress((int)(100.0 * stream.BaseStream.Position / stream.BaseStream.Length), $"Found WEBPs: {Files.Count}");

                long to_end = stream.BaseStream.Length - stream.BaseStream.Position;
                long old_pos = stream.BaseStream.Position;
                int size = to_end < Image_Extractor_Main_Form.BufferSize ? (int)to_end : (Image_Extractor_Main_Form.BufferSize > stream.BaseStream.Length) ? (int)stream.BaseStream.Length : Image_Extractor_Main_Form.BufferSize;

                byte[] arr = stream.ReadBytes(size);
                int offset_start = 0;
                int pos = 0;

                // Scanning of WEBP start
                while (offset_start != -1 && pos != arr.Length)
                {
                    pos = Utils.FindRangeInArray(ref arr, ref RIFF_Start, offset_start);
                    offset_start = pos;

                    if (pos != -1)
                    {
                        offset_start += 1; //PREVENTING INFINITE LOOP

                        int pos2 = Utils.FindRangeInArray(ref arr, ref WEBP_Start, offset_start);
                        if (pos2 == -1 || pos2 != pos + 8) // Skip RIFF and Size in header
                            continue;

                        long new_pos = old_pos + pos;
                        if (!Files.Any((ef) => ef.StartPos == new_pos))
                        {
                            Files.Add(new ExtractorFile() { StartPos = new_pos, Stream = stream, Size = BitConverter.ToUInt32(arr, pos + 4) + 8, Extension = ".webp" });
                        }
                    }
                }

                if (size > 12)
                    stream.BaseStream.Seek(-(12 - 1), SeekOrigin.Current);
            }

            res.IsSuccess = true;
            res.IsCancelledOrError = IsCancelled;
            res.Files = Files;
            res.ExtractorName = Name;
            return res;
        }
    }
}
