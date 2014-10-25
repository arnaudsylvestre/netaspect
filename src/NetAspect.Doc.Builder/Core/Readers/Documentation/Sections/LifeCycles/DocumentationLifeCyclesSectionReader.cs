using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects
{
    public static class DocumentationLifeCyclesSectionReader
    {
        public static LifeCyclesSectionModel ExtractLifeCycles(string directoryPath)
        {
            return new LifeCyclesSectionModel(
                CsTestFileReader.Read(Path.Combine(directoryPath, "PerTypeLifeCycleSampleTest.cs")),
                CsTestFileReader.Read(Path.Combine(directoryPath, "PerInstanceLifeCycleSampleTest.cs")),
                CsTestFileReader.Read(Path.Combine(directoryPath, "TransientLifeCycleSampleTest.cs"))
                );
        } 
    }
}