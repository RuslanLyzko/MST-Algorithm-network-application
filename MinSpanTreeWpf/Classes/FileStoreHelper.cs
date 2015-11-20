using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MinSpanTreeWpf.Classes
{
    public static class FileStoreHelper
    {
        private static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };

        public static void SaveToFile(string filePath, NodesWithEdges nodesWithEdges)
        {
            string json = JsonConvert.SerializeObject(nodesWithEdges, JsonSerializerSettings);
            File.WriteAllText(filePath, json);
        }

        public static NodesWithEdges LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<NodesWithEdges>(json, JsonSerializerSettings);
        }
    }
}
