using System.Collections.Generic;
using System.IO;
using System.Net;

namespace JadeStudio.Core.FileFormats.Bigfile
{
    public class FATFile
    {
        public static readonly int BIGcomp_M_BlockSize = 512000;

        public uint Offset;
        public int Key;
        public string Name;
        public string Path;

        public bool IsCompressed()
        {
            var key = Key.Hex();
            return key.StartsWith("F") && !key.StartsWith("F4") && !key.StartsWith("FF4");
        }

        public byte[] Read(BinaryReader reader)
        {
            reader.BaseStream.Position = Offset;

            var fileSize = reader.ReadInt32();
            var bytes = reader.ReadBytes(fileSize);

            if (!IsCompressed())
                return bytes;

            var decompressedBytes = new List<byte>();
            using (BinaryReader compReader = new BinaryReader(new MemoryStream(bytes)))
            {
                var outSize = 0;

                while (outSize == 0 || outSize == BIGcomp_M_BlockSize)
                {
                    outSize = compReader.ReadInt32();
                    var inSize = compReader.ReadInt32();

                    var compressed = compReader.ReadBytes(inSize);
                    var decompressed = new byte[outSize];

                    MiniLZO.MiniLZO.Decompress(compressed, decompressed);
                    decompressedBytes.AddRange(decompressed);
                }
            }

            return decompressedBytes.ToArray();
        }

        public void Write(BinaryWriter writer)
        {
            writer.BaseStream.Position = Offset;

            var content = File.ReadAllBytes(Path);

            if (!IsCompressed())
            {
                writer.Write(content.Length);
                writer.Write(content);
                return;
            }

            byte[] compressedBytes;
            int uncompressedSize = content.Length;
            using (BinaryReader reader = new BinaryReader(new MemoryStream(content)))
            {
                using (BinaryWriter compWriter = new BinaryWriter(new MemoryStream()))
                {
                    if (uncompressedSize <= BIGcomp_M_BlockSize)
                    {
                        var uncompressedBytes = reader.ReadBytes(uncompressedSize);

                        compWriter.Write(uncompressedSize);

                        var compressed = MiniLZO.MiniLZO.Compress(uncompressedBytes);
                        compWriter.Write(compressed.Length);
                        compWriter.Write(compressed);

                        compWriter.Write(new byte[(2048 - (compWriter.BaseStream.Position % 2048)) - 4]);
                    }
                    else
                    {
                        var remainingBytes = uncompressedSize;

                        while (remainingBytes > BIGcomp_M_BlockSize)
                        {
                            remainingBytes -= BIGcomp_M_BlockSize;

                            compWriter.Write(BIGcomp_M_BlockSize);

                            var compressed = MiniLZO.MiniLZO.Compress(reader.ReadBytes(BIGcomp_M_BlockSize));
                            compWriter.Write(compressed.Length);
                            compWriter.Write(compressed);
                        }

                        compWriter.Write(remainingBytes);
                        var lastCompressedChunk = MiniLZO.MiniLZO.Compress(reader.ReadBytes(BIGcomp_M_BlockSize));
                        compWriter.Write(lastCompressedChunk.Length);
                        compWriter.Write(lastCompressedChunk);

                        compWriter.Write(new byte[(2048 - (compWriter.BaseStream.Position % 2048)) - 4]);
                    }

                    compressedBytes = ((MemoryStream) compWriter.BaseStream).ToArray();
                }
            }

            writer.Write(compressedBytes.Length);
            writer.Write(compressedBytes);
        }
    }
}