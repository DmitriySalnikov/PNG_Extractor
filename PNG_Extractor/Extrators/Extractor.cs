using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	public struct ExtractorResult
	{
		public string ExtractorName;
		public bool IsSuccess;
		public int ExportedCount;
		public int FoundCount;
	}

	public class ExtractorFile
	{
		public long StartPos;
		public long Size;
		public BinaryReader Stream;

		public bool Save(string name)
		{
			Stream.BaseStream.Seek(StartPos, SeekOrigin.Begin);
			long size = Size;

			string new_file_name = name;
			if (File.Exists(new_file_name))
				File.Delete(new_file_name);

			BinaryWriter writer;
			if (size > 0)
			{
				writer = new BinaryWriter(File.OpenWrite(name));
			}
			else
			{
				return false;
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

			return true;
		}
	}

	public class Extractor
	{
		protected List<ExtractorFile> Files;

		public virtual string Name { get; }

		public virtual ExtractorResult Extract(BinaryReader stream, string extract_directory, BackgroundWorker bg_worker = null)
		{
			return new ExtractorResult();
		}
	}
}
