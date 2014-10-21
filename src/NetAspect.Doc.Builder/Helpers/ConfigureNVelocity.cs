using System.Collections;
using System.Collections.Generic;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace NetAspect.Doc.Builder.Helpers
{
    public class ConfigureNVelocity
    {
        private class NVelocityEntry
        {
            public string Key { get; set; }
            public object Value { get; set; }
        }

        List<NVelocityEntry> entries = new List<NVelocityEntry>();

        public static ConfigureNVelocity With(string name, object value)
        {
            return new ConfigureNVelocity().AndWith(name, value);
        }

        public ConfigureNVelocity AndWith(string name, object value)
        {
            entries.Add(new NVelocityEntry
                {
                Key = name,
                Value = value
                });
            return this;
        }

        public string AndGenerateInto(string template)
        {
            var velocity = new VelocityEngine();
            var hashtable = new Hashtable();
            foreach (NVelocityEntry entry in entries)
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
