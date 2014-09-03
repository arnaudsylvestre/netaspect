using System.Collections;
using System.Collections.Generic;
using System.IO;
using Commons.Collections;
using NetAspect.Doc.Builder.Model;
using NVelocity;
using NVelocity.App;

namespace NetAspect.Doc.Builder.Helpers
{
   public class InterceptorDocumentation
   {
      public InterceptorDocumentation()
      {
         Parameters = new List<string>();
      }

      public string Name { get; set; }

      public string Kind
      {
         get { return InterceptorModelHelper.ExtractKind(Name).ToString(); }
      }

      public string Event
      {
         get { return InterceptorModelHelper.ExtractEvent(Name).ToString(); }
      }

      public List<string> Parameters { get; set; }
      public string Member { get; set; }

      public string CallCode { get; set; }

      public string AspectCode { get; set; }

      public string ClassToWeaveCode { get; set; }

      public string When { get; set; }
   }


   public class NVelocityHelper
   {
      public static string GenerateContent(string template, string key, object value)
      {
         return GenerateContent(
            template,
            new NVelocityEntry
            {
               Key = key,
               Value = value,
            });
      }

      public static string GenerateContent(string template, params NVelocityEntry[] entries)
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

      public class NVelocityEntry
      {
         public string Key { get; set; }
         public object Value { get; set; }
      }
   }
}
