using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Documentation;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;

namespace NetAspect.Doc.Builder.Model
{
    public class DocumentationPageFactory
    {
        public static Page CreateDocumentationPage(string baseFolder)
        {
            return new Page(
                "Documentation",
                Templates.Templates.DocumentationPage,
                "Documentation",
                DocumentationReader.Read(baseFolder)
                );
        }
    }
}