using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace JadeStudio.Core.FileFormats.Bigfile
{
    public class Bigfile
    {
        public BinaryReader Reader;
        public BinaryWriter Writer;

        public BigfileHeader Header;
        public FATHeader[] FATHeaders;
        public FATFile[] Files;

        public Bigfile(string path)
        {
            var fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Reader = new BinaryReader(fileStream);
            Writer = new BinaryWriter(fileStream);
        }

        public void Read()
        {
            Header = new BigfileHeader();
            Header.Read(Reader);

            FATHeaders = new FATHeader[Header.FatsCount];
            for (int i = 0; i < Header.FatsCount; i++)
            {
                FATHeaders[i] = new FATHeader();
                FATHeaders[i].Read(Reader);
            }

            var filesCount = FATHeaders[0].FilesCount;

            Files = new FATFile[filesCount];
            Reader.BaseStream.Position = FATHeaders[0].Offset;

            for (var i = 0; i < filesCount; i++)
            {
                var file = new FATFile {Offset = Reader.ReadUInt32(), Key = Reader.ReadInt32()};

                Files[i] = file;
            }

            //Go to the filenames
            Reader.BaseStream.Position += (Header.UnknownCount2 - Header.FilesCount) * 8;
            Console.WriteLine(Reader.BaseStream.Position.ToString("X8"));

            for (int i = 0; i < filesCount; i++)
            {
                var fatName = new FATName();
                fatName.Read(Reader);

                Files[i].Name = fatName.FileName;
            }
        }

        public void Write()
        {
            Console.WriteLine("Writing BigfileHeader");
            Header.Write(Writer);

            Console.WriteLine("Writing FATHeader(s)");
            foreach (var fatHeader in FATHeaders)
                fatHeader.Write(Writer);

            var fileContentOffset = (uint) (BigfileHeader.Size + FATHeader.Size + (Files.Length * 8));
            foreach (var fatFile in Files)
            {
                fatFile.Offset = fileContentOffset;
                fileContentOffset += (4 + (uint) new FileInfo(fatFile.Path).Length);

                Console.WriteLine("Writing " + fatFile.Key.Hex() + "'s content");
                fatFile.Write(Writer);
            }

            Console.WriteLine("Writing offsets and keys of the files");
            Writer.BaseStream.Position = FATHeaders[0].Offset;

            foreach (var fatFile in Files)
            {
                Writer.Write(fatFile.Offset);
                Writer.Write(fatFile.Key);
            }
        }
    }
}