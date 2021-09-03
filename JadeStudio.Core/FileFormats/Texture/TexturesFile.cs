using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace JadeStudio.Core.FileFormats.Texture
{
    public class TexturesFile
    {
        public BinaryReader Reader;

        private FileStream _fileStream;
        public List<Chunk> Chunks = new List<Chunk>();

        public void Read(string path)
        {
            _fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Reader = new BinaryReader(_fileStream);

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
            {
                var size = Reader.ReadInt32();
                var chunk = new Chunk();
                chunk.Read(Reader, size);

                Chunks.Add(chunk);
            }
        }

        private void Close()
        {
            Reader.Close();
            _fileStream.Close();
        }

        public void DumpTextures(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Chunks.ForEach(chunk => chunk.Index = Chunks.IndexOf(chunk));
            Chunks.ForEach(delegate(Chunk chunk)
            {
                if (chunk.TextureType == 0x2001) //TODO: Find some cleaner way
                {
                    chunk.TextureType = (short) TextureType.TGA;
                }
            });

            //Map textures to the font it's representing
            var fontDictionary = new Dictionary<Chunk, Chunk>();
            for (var i = 0; i < Chunks.Count; i++)
            {
                var chunk = Chunks[i];
                if (chunk.IsFontDesc)
                {
                    fontDictionary[Chunks[i - 1]] = chunk;
                }
            }

            Console.WriteLine("Total chunks: " + Chunks.Count);
            Console.WriteLine("Info chunks: " + Chunks.Where(c => c.Data == null || c.GetTextureType() == TextureType.Procedural && c.Data != null).ToList().Count);
            Console.WriteLine("Data chunks: " + Chunks.Where(c => c.Data != null && c.GetTextureType() != TextureType.PaletteLink && c.GetTextureType() != TextureType.Procedural
                                                                  && !c.IsPalette).ToList().Count);

            //Get palettes and palettized textures
            var palettes = Chunks.Where(c => c.IsPalette).ToList();
            var textures = Chunks.Where(c =>
            {
                var type = c.GetTextureType();
                return (type == TextureType.Palette8Bit || type == TextureType.Palette4Bit) && c.Data != null;
            }).ToList();

            //Get TGAs and link the chunks together
            var tgaList = Chunks.Where(c => c.GetTextureType() == TextureType.TGA && c.Data != null).ToList();
            textures.AddRange(tgaList);

            var tgaWithoutData = Chunks.Where(c => c.GetTextureType() == TextureType.TGA && c.Data == null).ToList();
            for (var i = 0; i < tgaWithoutData.Count; i++)
            {
                var tga = tgaWithoutData[i];
                tga.LinkedIndex = Chunks.IndexOf(tgaList[i]);
            }

            //Read PaletteLinks
            var links = Chunks.Where(c => c.GetTextureType() == TextureType.PaletteLink).ToList();
            var textureKeys = new List<string>();
            var paletteKeys = new List<string>();
            foreach (var link in links)
            {
                using (var linkReader = new BinaryReader(new MemoryStream(link.Data)))
                {
                    var textureKey = linkReader.ReadInt32();
                    var paletteKey = linkReader.ReadInt32();

                    paletteKeys.Add(paletteKey.ToString("X8"));
                    textureKeys.Add(textureKey.ToString("X8"));
                }
            }

            //Link palettes
            var linkedPalletes = new Dictionary<string, Chunk>();
            var distinctPaletteKeys = paletteKeys.Distinct().ToList();
            for (var i = 0; i < distinctPaletteKeys.Count; i++)
            {
                try
                {
                    linkedPalletes[distinctPaletteKeys[i]] = palettes[i];
                }
                catch (Exception e)
                {
                    Console.WriteLine("palettes.Count: " + palettes.Count);
                    Console.WriteLine("palettes[0].Offset: " + palettes[0].Offset);
                    throw;
                }
            }

            //Link textures
            var linkedTextures = new Dictionary<string, Chunk>();
            var distinctTextureKeys = textureKeys.Distinct().ToList();
            var textureHeadersWithoutData = Chunks.Where(c => (c.GetTextureType() == TextureType.Palette4Bit || c.GetTextureType() == TextureType.Palette8Bit) && c.Data == null).ToList();
            for (var i = 0; i < distinctTextureKeys.Count; i++)
            {
                linkedTextures[distinctTextureKeys[i]] = textureHeadersWithoutData[i];
            }

            //Dump other texture types
            var remainingTextures = Chunks.Where(c =>
            {
                var type = c.GetTextureType();
                return (type != TextureType.Palette8Bit && type != TextureType.Palette4Bit &&
                        type != TextureType.TGA && type != TextureType.PaletteLink &&
                        !c.IsPalette && !c.IsFontDesc) && c.Data != null;
            }).ToList();
            foreach (var remainingTexture in remainingTextures)
            {
                remainingTexture.Filename = Chunks.IndexOf(remainingTexture) + "_" + remainingTexture.GetTextureType() + ".bin";
                File.WriteAllBytes(folderPath + remainingTexture.Filename, remainingTexture.Data);
            }

            //Start dumping textures
            for (int i = 0; i < paletteKeys.Count + tgaList.Count; i++)
            {
                var texture = textures[i];
                var textureType = texture.GetTextureType();

                texture.Filename = Chunks.IndexOf(texture) + "_" + textureType + ".tga";

                //Dump corresponding fontdesc and link the fontdesc to its texture
                if (fontDictionary.TryGetValue(texture, out var fontDesc))
                {
                    var indexOf = Chunks.IndexOf(texture);
                    fontDesc.LinkedIndex = indexOf;
                    fontDesc.Filename = indexOf + "_FONTDESC.bin";
                    File.WriteAllBytes(folderPath + fontDesc.Filename, fontDesc.Data);
                }

                if (textureType == TextureType.TGA)
                {
                    File.WriteAllBytes(folderPath + texture.Filename, texture.Data);
                    continue;
                }

                if (i >= paletteKeys.Count)
                {
                    Console.WriteLine("No palette: " + i);
                    File.WriteAllBytes(folderPath + i + ".bin", texture.Data);
                    continue;
                }

                var textureHeaderChunk = linkedTextures[textureKeys[i]];
                var textureIndex = Chunks.IndexOf(texture);
                textureHeaderChunk.LinkedIndex = textureIndex; //Link texture chunk without data to the one with data
                texture.LinkedIndex = textureHeaderChunk.Index; //Link texture chunk with data to the one without data
                links[i].LinkedIndex = textureIndex; //Link texture with data to palette link

                var linkedPalette = linkedPalletes[paletteKeys[i]];
                var usesRGBA = linkedPalette.Data.Length == 0x400 || linkedPalette.Data.Length == 0x40;

                if (linkedPalette.Data.Length != 0x300 && linkedPalette.Data.Length != 0x400 && linkedPalette.Data.Length != 0x30 && linkedPalette.Data.Length != 0x40)
                {
                    Console.WriteLine("Weird palette: " + texture.Unk3.Hex() + " | FontDesc: " + linkedPalette.IsFontDesc);

                    File.WriteAllBytes(folderPath + i + ".bin", texture.Data);
                    File.WriteAllBytes(folderPath + i + "_palette_" + linkedPalette.IsFontDesc + ".bin", linkedPalette.Data);
                    continue;
                }

                //Parse the palettes
                var palList = new List<RGBA>();
                using (var reader = new BinaryReader(new MemoryStream(linkedPalette.Data)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        palList.Add(new RGBA {B = reader.ReadByte(), G = reader.ReadByte(), R = reader.ReadByte(), A = usesRGBA ? reader.ReadByte() : (byte?) null});
                    }
                }

                //Dump texture to TGA
                using (var writer = new BinaryWriter(File.OpenWrite(folderPath + texture.Filename)))
                {
                    //TGAHeader
                    writer.Write((byte) 0x00); //IDLength
                    writer.Write((byte) 0x00); //ColorMapType
                    writer.Write((byte) 0x02); //ImageType (Uncompressed, True-color Image)

                    //ColorMapSpecification
                    writer.Write((short) 0x00); //FirstIndexEntry
                    writer.Write((short) 0x00); //ColorMapLength
                    writer.Write((byte) 0x00); //ColorMapEntrySize

                    //ImageSpecification
                    writer.Write((short) 0); //XOrigin
                    writer.Write((short) 0); //YOrigin
                    writer.Write(texture.Width); //Width
                    writer.Write(texture.Height); //Height
                    writer.Write((byte) 32); //PixelDepth
                    writer.Write((byte) 8); //ImageDescriptor

                    //ImageData
                    using (var reader = new BinaryReader(new MemoryStream(texture.Data)))
                    {
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            var index = reader.ReadByte();
                            RGBA rgba;

                            if (textureType == TextureType.Palette4Bit)
                            {
                                var index1 = (index & 0b11110000) >> 4;
                                var index2 = (index & 0b00001111);

                                rgba = palList[index1];
                                writer.Write(rgba.B);
                                writer.Write(rgba.G);
                                writer.Write(rgba.R);
                                writer.Write(usesRGBA ? (byte) rgba.A : (byte) 0xFF);

                                var rgba2 = palList[index2];
                                writer.Write(rgba2.B);
                                writer.Write(rgba2.G);
                                writer.Write(rgba2.R);
                                writer.Write(usesRGBA ? (byte) rgba2.A : (byte) 0xFF);
                                continue;
                            }

                            rgba = palList[index];
                            writer.Write(rgba.B);
                            writer.Write(rgba.G);
                            writer.Write(rgba.R);
                            writer.Write(usesRGBA ? (byte) rgba.A : (byte) 0xFF);
                        }
                    }
                }
            }

            //Dump chunk info to json (used when rebuilding)
            var infoJSON = JsonConvert.SerializeObject(Chunks, Formatting.Indented);
            File.WriteAllText(folderPath + "Info.json", infoJSON);
            
            Close();
        }

        public void Write(string contentFolderPath, string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            var jsonChunks = JsonConvert.DeserializeObject<List<Chunk>>(File.ReadAllText(contentFolderPath + "Info.json"));
            Console.WriteLine("Total chunks in json: " + jsonChunks.Count);

            //Read special textures' data
            jsonChunks.Where(chunk => chunk.IsSpecial()).ToList().ForEach(chunk => chunk.Data = File.ReadAllBytes(contentFolderPath + chunk.Filename));

            var chunksToWrite = jsonChunks.Where(c => c.GetTextureType() == TextureType.PaletteLink && c.LinkedIndex != -1).ToList();
            var otherTextures = jsonChunks.Where(c => c.IsSpecial() || c.GetTextureType() == TextureType.TGA && c.Size == 32).ToList();

            foreach (var otherTexture in otherTextures)
            {
                chunksToWrite.Insert(otherTexture.Index, otherTexture);
            }

            var count = 0;
            using (var writer = new BinaryWriter(File.OpenWrite(filePath)))
            {
                foreach (var chunk in chunksToWrite)
                {
                    if (chunk.GetTextureType() == TextureType.PaletteLink)
                    {
                        var linkedChunk = jsonChunks[chunk.LinkedIndex];
                        linkedChunk.TextureType = (short) TextureType.TGA;
                        linkedChunk.Write(writer);

                        count++;
                        continue;
                    }

                    if (!chunk.IsSpecial())
                    {
                        chunk.TextureType = (short) TextureType.TGA;
                    }

                    chunk.Write(writer);
                    count++;
                }

                chunksToWrite = chunksToWrite.Except(chunksToWrite.Where(c => c.IsSpecial()).ToList()).ToList();

                var dataCount = 0;
                foreach (var chunk in chunksToWrite)
                {
                    var chunkToWrite = (chunk.GetTextureType() == TextureType.PaletteLink || chunk.GetTextureType() == TextureType.TGA) ? jsonChunks[chunk.LinkedIndex] : chunk;

                    chunkToWrite.Data = File.ReadAllBytes(contentFolderPath + chunkToWrite.Filename);
                    chunkToWrite.Write(writer);
                    if (!chunkToWrite.IsSpecial())
                    {
                        chunkToWrite.TextureType = (short) TextureType.TGA;
                    }

                    dataCount++;

                    var fontDescCount = -1;
                    foreach (var c in jsonChunks)
                    {
                        if (c.IsFontDesc)
                        {
                            var chunksBeforeTextures = (jsonChunks.Where(c2 => (c2.GetTextureType() == TextureType.PaletteLink) || c2.IsSpecial() || c2.Size == 32 || c2.IsPalette).ToList().Count) + fontDescCount;
                            var fontDescIndex = (jsonChunks[c.LinkedIndex].Index - chunksBeforeTextures);
                            if (dataCount == fontDescIndex)
                            {
                                Console.WriteLine("Writing fontdesc!");
                                c.Data = File.ReadAllBytes(contentFolderPath + c.Filename);
                                c.Write(writer);
                                count++;
                            }

                            fontDescCount++;
                        }
                    }

                    count++;
                }
            }

            Console.WriteLine("Wrote " + count + " chunks.");
        }
    }
}