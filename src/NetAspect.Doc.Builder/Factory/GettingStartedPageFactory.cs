using System.IO;
using NetAspect.Doc.Builder.Core.GettingStarted;

namespace NetAspect.Doc.Builder.Model
{
    public class GettingStartedPageFactory
    {
        public static Page CreateGettingStartedPage(string baseFolder)
        {
            var reader = new GettingStartedReader();
            var model = new GettingStartedPageModel();
            reader.Read(model, Path.Combine(baseFolder, @"GettingStarted\GettingStartedPart1Test.cs"), Path.Combine(baseFolder, @"GettingStarted\GettingStartedPart2Test.cs"));
            return new Page("Getting Started", Templates.Templates.GettingStartedPage, "GettingStarted", model);
        }
    }
}