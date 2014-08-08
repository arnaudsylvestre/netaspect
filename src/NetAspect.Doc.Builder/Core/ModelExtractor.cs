using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NetAspect.Doc.Builder.Core
{
    public class ModelExtractor
    {
        public static Dictionary<string, string> ReadModel(string modelContent)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(modelContent);
        }

        public static void SaveModel(Dictionary<string, string> content, string file)
        {
            var serializeObject = JsonConvert.SerializeObject(content);
            File.WriteAllText(file, serializeObject);
        }
    }
}