using System.IO;

namespace NetAspect.Doc.Builder.Model
{
    public class DocumentationPageFactory
    {
        public static Page CreateDocumentationPage(string baseFolder)
        {
            var extractor = new DocumentationFromTestExtractor();
            return new Page(
                "Documentation",
                Templates.Templates.DocumentationPage,
                "Documentation",
                new DocumentationPageModel
                    {
                        Interceptors = extractor.ExtractInterceptors(Path.Combine(baseFolder, @"Documentation\Interceptors")),
                        Weaving = extractor.ExtractWeaving(Path.Combine(baseFolder, @"Documentation\Weaving")),
                        Parameters = extractor.ExtractParameters(Path.Combine(baseFolder, @"Documentation\Parameters")),
                    });
        }
    }
}