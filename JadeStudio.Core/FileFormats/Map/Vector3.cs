using System.IO;

namespace JadeStudio.Core.FileFormats.Map
{
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public static Vector3 Read(BinaryReader reader)
        {
            var vector3 = new Vector3
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle()
            };

            return vector3;
        }

        public override string ToString()
        {
            return "X: " + X + " | Y: " + Y + " | Z: " + Z;
        }
    }
}