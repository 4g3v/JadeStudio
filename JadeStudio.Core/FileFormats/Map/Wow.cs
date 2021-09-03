using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JadeStudio.Core.FileFormats.Map
{
    public class Wow
    {
        public byte[] Data;
        public string Name;
        public Matrix Matrix;
        public byte[] OtherData;
        public List<int> GaoKeys;

        public List<Gao> Gaos;
        public List<Gao> ExtraGaos = new List<Gao>();
        private string _path;

        public Wow(string path)
        {
            _path = Path.GetFileNameWithoutExtension(path);
        }

        public void Read(BinaryReader reader)
        {
            var wowOffset = reader.BaseStream.Position.ToString("X");

            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                Console.WriteLine("Tried to read too many wows!");
                return;
            }

            var length = reader.ReadInt32();

            var magic = reader.ReadInt32().Hex();
            if (magic != "776F772E") //.wow
            {
                Console.WriteLine("Invalid wow magic in wow!");
                return;
            }

            Data = reader.ReadBytes(0xC);

            var nameBytes = reader.ReadBytes(0x3C);
            Name = nameBytes.ToCString();

            Console.WriteLine("Working with " + Name + ".wow at " + wowOffset);

            Matrix = Matrix.Read(reader);

            var unk2 = reader.ReadSingle();
            var unk3 = reader.ReadInt32();
            var unk4 = reader.ReadInt32();
            var unk5 = reader.ReadInt32();

            var worldGridKey = reader.ReadInt32();
            var worldGrid2Key = reader.ReadInt32();
            var groupKey = reader.ReadInt32();
            var allNetworksKey = reader.ReadInt32();
            var WOR_gaul_WorldText = reader.ReadInt32();

//            Console.WriteLine();
//            Console.WriteLine("worldGridKey: " + worldGridKey.ToString("X"));
//            Console.WriteLine("worldGrid2Key: " + worldGrid2Key.ToString("X"));
//            Console.WriteLine("groupKey: " + groupKey.ToString("X"));
//            Console.WriteLine("allNetworksKey: " + allNetworksKey.ToString("X"));
//            Console.WriteLine();

            if (worldGridKey != 0 && worldGridKey != -1)
            {
//                Console.WriteLine("WorldGrid");
                var worldGridBytes = reader.ReadBytes(reader.ReadInt32());
            }

            if (worldGrid2Key != 0 && worldGrid2Key != -1)
            {
//                Console.WriteLine("WorldGrid2");

                var worldGrid2Bytes = reader.ReadBytes(reader.ReadInt32());
            }

            if (groupKey != 0 && groupKey != -1)
            {
//                Console.WriteLine("GameObjectGroup");

                GaoKeys = new List<int>();
                Gaos = new List<Gao>();

                var gaosLength = reader.ReadInt32();
                var gaoCount = gaosLength / 4;
                for (int i = 0; i < gaoCount; i++)
                {
                    GaoKeys.Add(reader.ReadInt32());
                }
            }

            if (allNetworksKey != 0 && allNetworksKey != -1)
            {
//                Console.WriteLine("AllNetworks");

                var allNetworksBytes = reader.ReadBytes(reader.ReadInt32());
                var allNetworksBytes2 = reader.ReadBytes(reader.ReadInt32());
                if (worldGridKey != 0 && worldGridKey != -1)
                {
                    var allNetworksBytes3 = reader.ReadBytes(reader.ReadInt32());
                }
            }

            for (int i = 0; i < GaoKeys.Count; i++)
            {
                var gao = new Gao();
                if (gao.Read(reader))
                    Gaos.Add(gao);
                else
                    break;
            }

            Console.WriteLine("GaoKeys.Count: " + GaoKeys.Count);
            Console.WriteLine("Gaos.Count: " + Gaos.Count);

            if (Gaos.Count == 0)
            {
                Console.WriteLine();
                return;
            }

            var blocks = ReadBlocksUntil(reader, 0x776F772E);

            int extraGaoCount = 0;
            foreach (var bytes in blocks)
            {
                if (bytes.Length < 8)
                {
                    continue;
                }

                using (var bReader = new BinaryReader(new MemoryStream(bytes)))
                {
                    var size = bReader.ReadInt32();
                    if (bReader.ReadInt32().Hex() == Gao.MAGIC)
                    {
                        extraGaoCount++;
                        bReader.BaseStream.Position = 0;
                        
                        var gao = new Gao();
                        gao.Read(bReader);
                        ExtraGaos.Add(gao);
                    }
                }
            }

            Console.WriteLine("Read data blocks, including " + extraGaoCount + " extra gaos!");
            Console.WriteLine();
        }

        private List<byte[]> ReadBlocksUntil(BinaryReader reader, int magic)
        {
            var byteList = new List<byte[]>();

            while (true)
            {
                var offset = reader.BaseStream.Position;
                var size = reader.ReadInt32();
                
                var readMagic = reader.ReadInt32();
                if (readMagic == magic)
                {
                    reader.BaseStream.Position = offset;
                    break;
                }

                if (IsWeirdLength(size))
                {
                    byteList.Add(SeekBackAndReadBlock(reader, offset + 4));
                    continue;
                }

                reader.BaseStream.Position = offset + size + 4;

                if (reader.BaseStream.Position >= reader.BaseStream.Length)
                {
                    byteList.Add(SeekBackAndReadBlock(reader, offset));
                    break;
                }

                readMagic = reader.ReadInt32();
                if (readMagic == magic)
                {
                    byteList.Add(SeekBackAndReadBlock(reader, offset));
                    break;
                }

                byteList.Add(SeekBackAndReadBlock(reader, offset));
            }

            return byteList;
        }

        private bool IsWeirdLength(int length)
        {
            return length == 0x6B6E732E || length == 0x646D732E;
        }

        private byte[] SeekBackAndReadBlock(BinaryReader reader, long offset)
        {
            byte[] bytes;
            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                reader.BaseStream.Position = offset;
                var size = reader.ReadInt32();
                writer.Write(size);
                writer.Write(reader.ReadBytes(size));

                bytes = ((MemoryStream) writer.BaseStream).ToArray();
            }

            return bytes;
        }
    }
}