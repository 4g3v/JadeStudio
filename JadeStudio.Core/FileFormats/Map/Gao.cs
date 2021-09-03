using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JadeStudio.Core.FileFormats.Map
{
    public class Gao
    {
        public const string MAGIC = "6F61672E";

        public byte[] Data;
        
        public int Unk1;
        public int Flags;
        public int VisFlags;
        public byte Unk2;
        public byte Unk3;
        public byte Unk4;
        public byte Unk5;
        public byte Unk6;
        public byte Unk7;
        public Matrix Matrix;
        public string Name;

        public bool Read(BinaryReader reader)
        {
            var offset = reader.BaseStream.Position;

//            Console.WriteLine("Reading gao at " + reader.BaseStream.Position.ToString("X"));

            var length = reader.ReadInt32();
            while (length == 0)
            {
                length = reader.ReadInt32();
            }

            var magic = reader.ReadInt32();
            var data = reader.ReadBytes(length - 4);

            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(magic);
                writer.Write(data);

                Data = ((MemoryStream) writer.BaseStream).ToArray();
            }

            if (magic.Hex() != MAGIC)
            {
                Console.WriteLine("Invalid gao: 0x" + offset.ToString("X") + " | Going back in stream!");
                reader.BaseStream.Position = offset;
                return false;
            }

            using (var gReader = new BinaryReader(new MemoryStream(data)))
            {
                Unk1 = gReader.ReadInt32();
                Flags = gReader.ReadInt32();
                VisFlags = gReader.ReadInt32();
                Unk2 = gReader.ReadByte();
                Unk3 = gReader.ReadByte();
                Unk4 = gReader.ReadByte();
                Unk5 = gReader.ReadByte();
                Unk6 = gReader.ReadByte();
                Unk7 = gReader.ReadByte();

                Matrix = Matrix.Read(gReader);
//
//                var unk8 = gReader.ReadSingle();
//                var unk9 = gReader.ReadSingle();
//                var unk10 = gReader.ReadSingle();
//                var unk11 = gReader.ReadSingle();
//                var unk12 = gReader.ReadSingle();
//                var unk13 = gReader.ReadSingle();
//
//                var unk14 = gReader.ReadInt32();
//                var unk15 = gReader.ReadInt32();
//                var unk16 = gReader.ReadInt32();
//                var actionKitRelated = gReader.ReadInt32();
//
//                if ((unk14 != -1 && unk14 != 0) && 
//                    (actionKitRelated > 0) && 
//                    (unk15 == -1 || unk15 == 0) && 
//                    (unk16 == -1 || unk16 == 0))
//                {
//                    var nameLength = gReader.ReadInt32();
//                    var name = gReader.ReadCStringW1252();
//                    Console.WriteLine(name);
//                }
            }

            var nullIndexes = new List<int>();
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] == 0x00)
                    nullIndexes.Add(i);
            }

            var nameIndex = nullIndexes[nullIndexes.Count - 2] + 1;
            var nameBytes = data.Skip(nameIndex).Take(data.Length - nameIndex).ToArray();
            Name = Encoding.GetEncoding(1252).GetString(nameBytes);
            
            return true;
        }
    }
}