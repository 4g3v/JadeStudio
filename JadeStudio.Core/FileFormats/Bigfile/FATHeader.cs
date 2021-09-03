using System.IO;

namespace JadeStudio.Core.FileFormats.Bigfile
{
    public class FATHeader
    {
        public int FilesCount;
        public int Unknown;
        public uint Offset;
        public uint Unknown2;
        public int Unknown3;
        public int Unknown4;

        public static readonly int Size = 0x18;

        public void Read(BinaryReader bigReader)
        {
            FilesCount = bigReader.ReadInt32();
            Unknown = bigReader.ReadInt32();
            Offset = bigReader.ReadUInt32();
            Unknown2 = (uint) bigReader.ReadInt32();
            Unknown3 = bigReader.ReadInt32();
            Unknown4 = bigReader.ReadInt32();
        }

        public void Write(BinaryWriter bigWriter)
        {
            bigWriter.Write(FilesCount);
            bigWriter.Write(Unknown);
            bigWriter.Write(Offset);
            bigWriter.Write(Unknown2);
            bigWriter.Write(Unknown3);
            bigWriter.Write(Unknown4);
        }
    }
}