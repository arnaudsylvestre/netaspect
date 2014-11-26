using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects
{
    public static class DocumentationOtherAssembliesSectionReader
    {
        public static OtherAssembliesSectionModel ExtractOtherAssemblies(string directoryPath)
        {
            return new OtherAssembliesSectionModel(
                CsTestFileReader.Read(Path.Combine(directoryPath, "SampleAnotherAssemblyTest.cs"))
                );
        }
    }
}