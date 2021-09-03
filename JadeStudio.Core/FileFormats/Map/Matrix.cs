using System.IO;

namespace JadeStudio.Core.FileFormats.Map
{
    public class Matrix
    {
        public float[] m0;
        public float[] m1;
        public float[] m2;
        public float[] m3;

        public int Type;

        public static Matrix Read(BinaryReader reader)
        {
            var matrix = new Matrix
            {
                m0 = new[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                },
                m1 = new[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                },
                m2 = new[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                },
                m3 = new[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                },
                Type = reader.ReadInt32()
            };

            return matrix;
        }

        public override string ToString()
        {
            return "[" + m0[0] + ", " + m0[1] + ", " + m0[2] + ", " + m0[3] + "]\n"
                   + "[" + m1[0] + ", " + m1[1] + ", " + m1[2] + ", " + m1[3] + "]\n"
                   + "[" + m2[0] + ", " + m2[1] + ", " + m2[2] + ", " + m2[3] + "]"
                   + "[" + m3[0] + ", " + m3[1] + ", " + m3[2] + ", " + m3[3] + "]\n";
        }
    }
}