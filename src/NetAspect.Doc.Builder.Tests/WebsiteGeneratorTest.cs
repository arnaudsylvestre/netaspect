using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NetAspect.Doc.Builder.Core;
using NetAspect.Doc.Builder.Model;
using NUnit.Framework;

namespace NetAspect.Doc.Builder.Tests
{
   [TestFixture]
   public class WebsiteGeneratorTest
   {
      [Test]
      public void CheckWebsiteGeneration()
      {
         //string baseDirectory = @"D:\Sources\3rdParty\fluentaspect-git\fluentaspect\";
         string baseDirectory = @"D:\Developpement\netaspect\";
         string tests = baseDirectory + @"..\netaspect-site";
         if (!Directory.Exists(tests))
            Directory.CreateDirectory(tests);
         
         var generator = new WebsiteGenerator();
         WebSite webSite = WebsiteFactory.Create(baseDirectory + @"src\NetAspect.Weaver.Tests\docs\");
         generator.Generate(webSite, tests);
      }
   }
}
