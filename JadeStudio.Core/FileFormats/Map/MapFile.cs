using System;
using System.Collections.Generic;
using System.IO;

namespace JadeStudio.Core.FileFormats.Map
{
    public class MapFile
    {
        public List<int> WowKeys;
        public List<Wow> Wows;
        
        public void Read(string path)
        {
            Console.WriteLine("Working with " + path);
            
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                //Merged WOW header
                WowKeys = new List<int>();
                var wowKeysLength = reader.ReadInt32();
                for (var i = 0; i < wowKeysLength / 8; i++)
                {
                    var wowKey = reader.ReadInt32();
                    var wowMagic = reader.ReadInt32().Hex();
                    if (wowMagic != "776F772E") //.wow
                    {
                        Console.WriteLine("Invalid wow magic in merged wow header!");
                        return;
                    }
                    
                    WowKeys.Add(wowKey);
                }
                
                Wows = new List<Wow>();
                for (int i = 0; i < wowKeysLength / 8; i++)
                {
                    var wow = new Wow(path);
                    wow.Read(reader);
                    
                    Wows.Add(wow);
                }
            }
        }
    }
}