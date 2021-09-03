using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using JadeStudio.Core.FileFormats.Texture;

namespace JadeStudio.Core.FileFormats.Text
{
    public class TextFile
    {
        public List<TextGroup> FirstTextGroups = new List<TextGroup>();
        public List<TextGroup> TextGroups = new List<TextGroup>();
        
        public void Read(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                FirstTextGroups = TextGroup.ReadGroups(reader);
                
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var groups = TextGroup.ReadGroups(reader);
                    TextGroups.AddRange(groups);
                }
            }
        }

        public void Write(string path)
        {
            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                TextGroup.WriteGroups(writer, FirstTextGroups);
                TextGroup.WriteGroups(writer, TextGroups);
            }
        }
    }
}