using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Json
{
    internal static class JsonBase
    {
        public static void SaveBase(List<BaseClient> clients, string path)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
                
            };
            var jsonString = JsonConvert.SerializeObject(clients, Formatting.Indented, setting);
            
            using (StreamWriter fs = new StreamWriter(path))
            {
                fs.WriteLine(jsonString);
            }
        }

        public static List<BaseClient> LoadBase(string Path)
        {
            if (!File.Exists(Path))
            {
                using (FileStream fs = File.Create(Path))
                {

                }
            }
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            List<BaseClient> baseClients = new List<BaseClient>();
            using (StreamReader streamReader = new StreamReader(Path))
            {
                baseClients = JsonConvert.DeserializeObject<List<BaseClient>>(streamReader.ReadToEnd(), setting);
            }
            return baseClients;
        }
    }
}
