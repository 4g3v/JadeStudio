using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace JadeStudio.Core.FileFormats.Texture
{
    public class Chunk
    {
        public int Index;

        public int Size;
        public int Unk1;
        public short Unk2;
        public short TextureType;
        public short Width;
        public short Height;
        public int Unk3;
        public int FontID;
        public int Magic1;
        public int Magic2;
        public int Magic3;

        [JsonIgnore] public byte[] Data;
        public bool IsPalette;

        public bool IsFontDesc;

        public string Offset;
        public string Filename;
        public int LinkedIndex = -1;

        public TextureType GetTextureType()
        {
            return (TextureType) TextureType;
        }

        public bool IsSpecial()
        {
            return GetTextureType() == Texture.TextureType.Procedural || GetTextureType() == Texture.TextureType.SpriteGen || GetTextureType() == Texture.TextureType.Animated;
        }

        public void Read(BinaryReader reader, int size)
        {
            Size = size;

            Offset = reader.BaseStream.Position.ToString("X16");

            reader.BaseStream.Position += 6;
            var type = reader.ReadInt16();
            reader.BaseStream.Position -= (6 + 2);

            reader.BaseStream.Position += 0x14;
            var magics = reader.ReadBytes(12);
            reader.BaseStream.Position -= (0x14 + 12);

            if (magics.ToHex() == "3412D0CAFF00FF00DEC0DEC0")
            {
                Unk1 = reader.ReadInt32();
                Unk2 = reader.ReadInt16();
                TextureType = reader.ReadInt16();
                Width = reader.ReadInt16();
                Height = reader.ReadInt16();
                Unk3 = reader.ReadInt32();
                FontID = reader.ReadInt32();
                Magic1 = reader.ReadInt32();
                Magic2 = reader.ReadInt32();
                Magic3 = reader.ReadInt32();

                var remainingBytes = size - 0x20;
                if (remainingBytes != 0)
                {
                    //Needed for some procedural textures, they don't seem to follow the format.
                    if (GetTextureType() == Texture.TextureType.Procedural)
                    {
                        if (remainingBytes == 64)
                        {
                            remainingBytes += 4;
                        }
                    }

                    Data = reader.ReadBytes(remainingBytes);
                }
            }
            else
            {
                Data = reader.ReadBytes(size);
                if (Encoding.ASCII.GetString(Data.Take(8).ToArray()) == "FONTDESC")
                {
                    IsFontDesc = true;
                    return;
                }

                IsPalette = type != 7;
            }
        }

        public void Write(BinaryWriter writer)
        {
            byte[] content;

            using (var memWriter = new BinaryWriter(new MemoryStream()))
            {
                if (IsFontDesc || IsPalette)
                {
                    memWriter.Write(Data);
                }
                else
                {
                    memWriter.Write(Unk1);
                    memWriter.Write(Unk2);
                    memWriter.Write(TextureType);
                    memWriter.Write(Width);
                    memWriter.Write(Height);
                    memWriter.Write(Unk3);
                    memWriter.Write(FontID);
                    memWriter.Write(Magic1);
                    memWriter.Write(Magic2);
                    memWriter.Write(Magic3);
                    if (Data != null)
                        memWriter.Write(Data);
                }

                content = ((MemoryStream) memWriter.BaseStream).ToArray();
            }

            if (GetTextureType() == Texture.TextureType.Procedural && Data?.Length == 68)
            {
                Console.WriteLine("Writing weird procedural texture.");
                writer.Write(content.Length - 4);
            }
            else
            {
                writer.Write(content.Length);
            }

            writer.Write(content);
        }
    }
}