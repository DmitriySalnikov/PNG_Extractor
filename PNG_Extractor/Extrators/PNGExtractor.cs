using System.Collections.Generic;
using System.IO;

namespace PNG_Extractor.Extrators
{
    public class PNGExtractor : Extractor
    {
        byte[] PNG_Start = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
        byte[] PNG_End = new byte[] { 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82 };

        public PNGExtractor()
        {
        }

        public PNGExtractor(BackgroundWorkerCustom bgw) : base(bgw)
        {
        }

        public override string Name => "PNG Extractor";

        public override ExtractorResult Extract(BinaryReader stream, string extract_directory)
        {
            ReportInit();
            var res = new ExtractorResult() { ExtractorName = Name, IsSuccess = false, IsCancelledOrError = true };

            Files = new List<ExtractorFile>();

            bool is_found_start_of_png = false;
            bool is_first_try_to_found_start = true;
            long prev_png_start = 0;
            //long max_position = 0; //For prevent progress bar moving backward

            // Scanning of PNGs in whole file
            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                //max_position = Math.Max(stream.BaseStream.Position, max_position);

                if (IsCancelled) { return res; }
                if (IsStopScanning)
                {
                    if (Files[Files.Count - 1].Size == 0)
                        Files.RemoveAt(Files.Count - 1);
                    break;
                }
                ReportProgress((int)(100.0 * stream.BaseStream.Position / stream.BaseStream.Length), $"Found PNGs: {(is_found_start_of_png ? Files.Count - 1 : Files.Count)}");

                long to_end = stream.BaseStream.Length - stream.BaseStream.Position;
                long old_pos = stream.BaseStream.Position;
                int size = to_end < PNG_Extractor.BufferSize ? (int)to_end : (PNG_Extractor.BufferSize > stream.BaseStream.Length) ? (int)stream.BaseStream.Length : PNG_Extractor.BufferSize;

                byte[] arr = stream.ReadBytes(size);
                int offset_start = 0;
                int offset_end = 0;

                int pos = 0;
                // Scanning of PNG start and end in block
                while (is_found_start_of_png ? offset_end != -1 : offset_start != -1 && pos != arr.Length)
                {
                    pos = 0;
                    if (!is_found_start_of_png && offset_start != -1)
                    {
                        pos = Utils.FindRangeInArray(ref arr, ref PNG_Start, offset_start);
                        offset_start = pos;
                        if (pos != -1)
                        {
                            offset_start += 1; //PREVENTING INFINITE LOOP

                            long new_pos = old_pos + pos;
                            bool contains = false;
                            foreach (var f in Files)
                            {
                                if (f.StartPos == new_pos)
                                {
                                    contains = true;
                                    break;
                                }
                            }

                            if (!contains)
                            {
                                if (Files.Count > 0)
                                    prev_png_start = Files[Files.Count - 1].StartPos;

                                Files.Add(new ExtractorFile() { StartPos = new_pos, Stream = stream });
                                is_found_start_of_png = true;
                            }
                            else
                            {

                            }
                        }
                    }

                    if (is_found_start_of_png && offset_end != -1)
                    {
                        pos = Utils.FindRangeInArray(ref arr, ref PNG_End, offset_end, true);
                        offset_end = pos;
                        if (pos != -1)
                        {
                            long new_position = old_pos + pos;
                            var file = Files[Files.Count - 1];
                            if (new_position > file.StartPos)
                            {
                                file.Size = new_position - file.StartPos;
                                is_found_start_of_png = false;
                                is_first_try_to_found_start = true;
                                break;
                            }
                        }
                    }
                }

                if (size > PNG_Start.Length)
                {
                    if (is_first_try_to_found_start && !is_found_start_of_png && Files.Count > 0)
                    {
                        stream.BaseStream.Seek(prev_png_start + PNG_Start.Length, SeekOrigin.Begin);
                        is_first_try_to_found_start = false;
                    }
                    else
                    {
                        stream.BaseStream.Seek(-(PNG_Start.Length - 1), SeekOrigin.Current);
                    }
                }
            }

            if (Files.Count > 0)
                Directory.CreateDirectory(extract_directory);

            int i = 0;
            int j = 0;
            foreach (var f in Files)
            {
                if (IsCancelled) { return res; }
                ReportProgress((int)(100 * j / Files.Count), $"Current file: {j}, Saved files: {i}, Total files: {Files.Count}");

                if (f.Save(Path.Combine(extract_directory, $"{i}.png")) == SaveExtractedFileError.OK)
                {
                    i++;
                }
                j++;
            }

            res.IsSuccess = i > 0;
            res.IsCancelledOrError = IsCancelled;
            res.ExportedCount = i;
            res.FoundCount = Files.Count;
            res.ExtractorName = Name;
            return res;
        }
    }
}
