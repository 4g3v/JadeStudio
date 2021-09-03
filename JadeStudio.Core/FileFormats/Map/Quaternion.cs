using System.IO;

namespace JadeStudio.Core.FileFormats.Map
{
    public class Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static Quaternion Read(BinaryReader reader)
        {
            var quat = new Quaternion
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                W = reader.ReadSingle()
            };

            return quat;
        }
        
        public override string ToString()
        {
            return "X: " + X + " | Y: " + Y + " | Z: " + Z + " | W: " + W;
        }
    }
}