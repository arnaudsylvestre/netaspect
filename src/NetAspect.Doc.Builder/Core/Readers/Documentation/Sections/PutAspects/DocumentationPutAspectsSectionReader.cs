using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects
{
    public static class DocumentationPutAspectsSectionReader
    {
        public static PutAspectsSectionModel ExtractPutAspects(string directoryPath)
        {
            return new PutAspectsSectionModel(
                CsTestFileReader.Read(Path.Combine(directoryPath, "WeaveWithAttributeSampleTest.cs")),
                CsTestFileReader.Read(Path.Combine(directoryPath, "WeaveWithSelectSampleTest.cs"))
                );
        } 
    }
}