using System.Collections;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace NetAspect.Doc.Builder.Helpers
{
    public class NVelocityHelper
    {
        public class NVelocityEntry
        {
            public string Key { get; set; }
            public object Value { get; set; }
        }

        public static string GenerateContent(string template, string key, object value)
        {
            return GenerateContent(template, new NVelocityEntry()
                {
                    Key = key,
                    Value = value,
                });
        }

        public static string GenerateContent(string template, params NVelocityEntry[] entries)
        {
            var velocity = new VelocityEngine();
            var hashtable = new Hashtable();
            foreach (var entry in entries)
            {
                hashtable.Add(entry.Key, entry.Value);
                
            }
            var context = new VelocityContext(hashtable);
            var props = new ExtendedProperties();
            velocity.Init(props);

            var writer = new StringWriter();
            velocity.Evaluate(context, writer, "", template);
            return writer.ToString();
        }
    }
}