using System.IO;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Core.Readers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
   public class GettingStartedReader
   {
      public static void Read(GettingStartedPageModel page, string gettingStartedPat1File, string gettingStartedPat2File)
      {
          CsFileReader.ReadCsFile<GettingStartedSection1Visitor, GettingStartedPageModel>(page, gettingStartedPat1File);
          CsFileReader.ReadCsFile<GettingStartedSection2Visitor, GettingStartedPageModel>(page, gettingStartedPat2File);
      }
   }
}
