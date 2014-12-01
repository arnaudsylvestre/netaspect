using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Documentation;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;

namespace NetAspect.Doc.Builder.Model
{
    public class SamplesPageFactory
    {
        public static Page CreateSamplesPage(string baseFolder)
        {
            return new Page(
                "Samples",
                Templates.Templates.SamplesPage,
                "Samples",
                SamplesReader.Read(baseFolder)
                );
        }
    }
}