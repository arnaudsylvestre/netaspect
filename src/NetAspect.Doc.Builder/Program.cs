using System.IO;
using NetAspect.Doc.Builder.Core;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder
{
   internal class Program
   {
      private static void Main(string[] args)
      {
          string tests = args[1];
          if (!Directory.Exists(tests))
              Directory.CreateDirectory(tests);

          var generator = new WebsiteGenerator();
          WebSite webSite = WebsiteFactory.Create(args[0]);
          generator.Generate(webSite, tests);
      }
   }
}
