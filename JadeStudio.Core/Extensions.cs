using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JadeStudio.Core
{
    public static class Extensions
    {
        public static string ReadCStringUTF8(this BinaryReader reader)
        {
            var byteList = new List<byte?>();
            byte? lastByte = null;

            while (lastByte != 0x00)
            {
                lastByte = reader.ReadByte();

                if (lastByte != 0x00)
                    byteList.Add(lastByte);
            }

            return Encoding.UTF8.GetString(Array.ConvertAll(byteList.ToArray(), x => x ?? 0));
        }

        public static string ReadCStringW1252(this BinaryReader reader)
        {
            var byteList = new List<byte?>();
            byte? lastByte = null;

            while (lastByte != 0x00)
            {
                lastByte = reader.ReadByte();

                if (lastByte != 0x00)
                    byteList.Add(lastByte);
            }

            return Encoding.GetEncoding(1252).GetString(Array.ConvertAll(byteList.ToArray(), x => x ?? 0));
        }

        public static void WriteCString(this BinaryWriter writer, string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                writer.Write((byte) s[i]);
            }

            writer.Write((byte) 0x00);
        }

        public static void WriteCStringW1252(this BinaryWriter writer, string s)
        {
            var w1252Bytes = Encoding.GetEncoding(1252).GetBytes(s);

            for (var i = 0; i < w1252Bytes.Length; i++)
            {
                writer.Write(w1252Bytes[i]);
            }

            writer.Write((byte) 0x00);
        }

        public static string ToCString(this byte[] data)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                var c = data[i];
                if (c == 0x00)
                    break;

                sb.Append((char)c);
            }

            return sb.ToString();
        }

        public static string Hex(this int i)
        {
            return i.ToString("X8");
        }

        public static string Hex(this uint i)
        {
            return i.ToString("X8");
        }

        public static string ToHex(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);

            return hex.ToString();
        }

        public static int IntFromHex(this string s)
        {
            return int.Parse(s, NumberStyles.HexNumber);
        }

        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static void AppendChar(this RichTextBox box, char c, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(Char.ToString(c));
            box.SelectionColor = box.ForeColor;
        }
    }
}