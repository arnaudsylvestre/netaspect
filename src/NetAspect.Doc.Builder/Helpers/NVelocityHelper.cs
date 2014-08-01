using System.Collections;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace NetAspect.Doc.Builder.Helpers
{
    public class NVelocityHelper
    {
        public static string GenerateContent(string key, object value, string template)
        {
            var velocity = new VelocityEngine();
            var hashtable = new Hashtable();
            hashtable.Add(key, value);
            var context = new VelocityContext(hashtable);
            var props = new ExtendedProperties();
            velocity.Init(props);

            var writer = new StringWriter();
            velocity.Evaluate(context, writer, "", template);
            return writer.ToString();
        }
    }
}