using NUnit.Framework;
using NetAspect.Doc.Builder.Core;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Tests
{
    [TestFixture]
    public class WebsiteGenerateTest
    {
        [Test]
         public void CheckWebsiteGeneration()
         {
             var generator = new WebsiteGenerator();

            var website = new WebSite();
            website.Pages.Add(new HomePage());
            generator.Generate(website, "");
         }
    }
}