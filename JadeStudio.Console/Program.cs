using System;
using System.IO;
using JadeStudio.Core.FileFormats.Map;
using static System.Console;

namespace JadeStudio.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            WriteLine("Texture filename:");
//            var filename = ReadLine();
//
//            WriteLine("(E)xtract|(B)uild:");
//            var key = ReadKey().Key;
//            WriteLine();
//
//            if (key == ConsoleKey.B)
//            {
//                var texturesFile2 = new TexturesFile();
//                texturesFile2.Write("Tex\\" + filename + "\\", filename + ".bin");
//            }
//            else if (key == ConsoleKey.E)
//            {
//                var texturesFile = new TexturesFile();
//                texturesFile.Read(filename + ".bin");
//                texturesFile.DumpTextures("Tex\\" + filename + "\\");
//            }
//
//            WriteLine("Finished!");

//            var textFile = new TextFile();
//            textFile.Read("FD20631B_fd20631b.bin");
//            
//            var textFile2 = new TextFile();
//            textFile2.FirstTextGroups = textFile.FirstTextGroups;
//            textFile2.TextGroups = textFile.TextGroups;
//            textFile2.Write("temp.bin");

            CreateDir("Gao");

            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "FF0*.bin"))
            {
                var mapFile = new MapFile();
                mapFile.Read(file);
                
                WriteLine("Successfully parsed " + file);
                WriteLine();

//                var mapWowFolderPath = "Gao\\" + Path.GetFileNameWithoutExtension(file) + "\\";
//                CreateDir(mapWowFolderPath);
//                
//                if (mapFile.Wows == null)
//                {
//                    continue;
//                }
//                
//                for (var i = 0; i < mapFile.Wows.Count; i++)
//                {
//                    var wow = mapFile.Wows[i];
//                    var wowFolderPath = mapWowFolderPath + i + ";" + wow.Name + "\\";
//                    CreateDir(wowFolderPath);
//
//                    for (var j = 0; j < wow.Gaos.Count; j++)
//                    {
//                        File.WriteAllBytes(wowFolderPath + "Normal_" + j + ".gao", wow.Gaos[j].Data);
//                    }
//                    for (var j = 0; j < wow.ExtraGaos.Count; j++)
//                    {
//                        File.WriteAllBytes(wowFolderPath + "Extra_" + j + ".gao", wow.ExtraGaos[j].Data);
//                    }
//                }
            }
        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}