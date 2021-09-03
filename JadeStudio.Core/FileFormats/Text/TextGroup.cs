using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JadeStudio.Core.FileFormats.Text
{
    public class TextGroup
    {
        public int ID;
        public List<Tuple<int, int>> TxiTxsEntries;
        public int Unknown;
        public List<int> BreakIndexes;
        public List<string> Texts;

        public static List<TextGroup> ReadGroups(BinaryReader reader)
        {
            var textGroups = new List<TextGroup>();

            var txgLength = reader.ReadInt32();
            var txgCount = txgLength / 8;
            for (int i = 0; i < txgCount; i++)
            {
                var txgID = reader.ReadInt32();
                var txgMagic = reader.ReadBytes(4);

                textGroups.Add(new TextGroup {ID = txgID});
            }

            for (int i = 0; i < txgCount; i++)
            {
                textGroups[i].TxiTxsEntries = new List<Tuple<int, int>>();
                
                var txiTxsLength = reader.ReadInt32();
                for (int j = 0; j < txiTxsLength / 16; j++)
                {
                    var txiID = reader.ReadInt32();
                    var txiMagic = reader.ReadBytes(4);
                    var txsID = reader.ReadInt32();
                    var txsMagic = reader.ReadBytes(4);

                    textGroups[i].TxiTxsEntries.Add(new Tuple<int, int>(txiID, txsID));
                }

                textGroups[i].Unknown = reader.ReadInt32();
            }

            for (int i = 0; i < txgCount; i++)
            {
                var breakIndexesLength = reader.ReadInt32();

                textGroups[i].BreakIndexes = new List<int>();
                for (int j = 0; j < breakIndexesLength / 4; j++)
                {
                    textGroups[i].BreakIndexes.Add(reader.ReadInt32());
                }

                var textsLength = reader.ReadInt32();
                var textBytes = reader.ReadBytes(textsLength);

                using (var textReader = new BinaryReader(new MemoryStream(textBytes)))
                {
                    textGroups[i].Texts = new List<string>();

                    foreach (var breakIndex in textGroups[i].BreakIndexes)
                    {
                        textBytes = textBytes.Skip(breakIndex).ToArray();

                        var text = textReader.ReadCStringW1252();
                        textGroups[i].Texts.Add(text);
                    }
                }
            }

            return textGroups;
        }

        public static void WriteGroups(BinaryWriter writer, List<TextGroup> groups)
        {
            writer.Write(groups.Count * 8);
            
            foreach (var textGroup in groups)
            {
                writer.Write(textGroup.ID);
                writer.Write(new byte[]{0x2E, 0x74, 0x78, 0x67}); //.txg
            }
            
            foreach (var textGroup in groups)
            {
                writer.Write(textGroup.TxiTxsEntries.Count * 16);
                foreach (var txiTxsEntry in textGroup.TxiTxsEntries)
                {
                    writer.Write(txiTxsEntry.Item1);
                    writer.Write(new byte[] {0x2E, 0x74, 0x78, 0x69}); //.txi
                    writer.Write(txiTxsEntry.Item2);
                    writer.Write(new byte[] {0x2E, 0x74, 0x78, 0x73}); //.txs
                }
                
                writer.Write(textGroup.Unknown);
            }

            foreach (var textGroup in groups)
            {
                var breakIndexes = new List<int>();
                
                byte[] textBytes;
                using (var textWriter = new BinaryWriter(new MemoryStream()))
                {
                    foreach (var text in textGroup.Texts)
                    {
                        breakIndexes.Add((int) textWriter.BaseStream.Position);
                        textWriter.WriteCStringW1252(text);
                    }

                    textBytes = ((MemoryStream) textWriter.BaseStream).ToArray();
                }
                
                writer.Write(breakIndexes.Count * 4);
                foreach (var breakIndex in breakIndexes)
                {
                    writer.Write(breakIndex);
                }
                
                writer.Write(textBytes.Length);
                writer.Write(textBytes);
            }
        }
    }
}