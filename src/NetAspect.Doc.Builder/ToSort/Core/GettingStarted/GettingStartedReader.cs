using System.IO;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
   public class GettingStartedReader
   {
      private readonly CSharpParser parser = new CSharpParser();

      public void Read(GettingStartedPageModel page, string gettingStartedPat1File, string gettingStartedPat2File)
      {
         ReadGettingStartedPage1(page, gettingStartedPat1File);
         ReadGettingStartedPage2(page, gettingStartedPat2File);
      }

      private void ReadGettingStartedPage1(GettingStartedPageModel page, string gettingStartedPat1File)
      {
         using (FileStream stream = File.OpenRead(gettingStartedPat1File))
         {
            SyntaxTree syntaxTree = parser.Parse(stream);
            syntaxTree.AcceptVisitor(new GettingStartedPage1Visitor(page));
         }
      }

      private void ReadGettingStartedPage2(GettingStartedPageModel page, string gettingStartedPart2File)
      {
         using (FileStream stream = File.OpenRead(gettingStartedPart2File))
         {
            SyntaxTree syntaxTree = parser.Parse(stream);
            syntaxTree.AcceptVisitor(new GettingStartedPage2Visitor(page));
         }
      }
   }
}
