using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Core.Readers.Core
{
    public static class CsTestFileReader
    {
         public static CsTestFile Read(string filePath)
         {
             return CsFileReader.ReadCsFile<CsTestFileVisitor, CsTestFile>(filePath);
         }
    }
}