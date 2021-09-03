using System.IO;

namespace JadeStudio.Core.FileFormats.Bigfile
{
    public class FATName
    {
        public int Unknown;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public string FileName;

        public void Read(BinaryReader reader)
        {
            Unknown = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            FileName = reader.ReadCStringUTF8();

            reader.BaseStream.Position += (64 - (FileName.Length + 1));
        }
    }
}