using System;
using System.IO;
using System.Text;

namespace JadeStudio.Core.FileFormats.Bigfile
{
    public class BigfileHeader
    {
        public string Magic;
        public int ID;
        public int FilesCount;
        public int UnknownCount;
        public byte[] Unknown;
        public int UnknownCount2;
        public int FatsCount;
        public int StartKey;

        public static readonly int Size = 0x2C;

        public void Read(BinaryReader bigReader)
        {
            Magic = bigReader.ReadCStringUTF8();
            ID = bigReader.ReadInt32();
            FilesCount = bigReader.ReadInt32();
            UnknownCount = bigReader.ReadInt32();
            Unknown = bigReader.ReadBytes(16);
            UnknownCount2 = bigReader.ReadInt32();
            FatsCount = bigReader.ReadInt32();
            StartKey = bigReader.ReadInt32();
        }

        public void Write(BinaryWriter bigWriter)
        {
            bigWriter.WriteCString(Magic);
            bigWriter.Write(ID);
            bigWriter.Write(FilesCount);
            bigWriter.Write(UnknownCount);
            bigWriter.Write(Unknown);
            bigWriter.Write(UnknownCount2);
            bigWriter.Write(FatsCount);
            bigWriter.Write(StartKey);
        }
    }
}