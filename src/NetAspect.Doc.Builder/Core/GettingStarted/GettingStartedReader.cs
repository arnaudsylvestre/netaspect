using System.IO;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
    public class GettingStartedReader
    {
        CSharpParser parser = new CSharpParser();

        public void Read(GettingStartedPage page, string gettingStartedPat1File, string gettingStartedPat2File)
        {
            ReadGettingStartedPage1(page, gettingStartedPat1File);
            ReadGettingStartedPage2(page, gettingStartedPat2File);
            
        }

        private void ReadGettingStartedPage1(GettingStartedPage page, string gettingStartedPat1File)
        {
            using (var stream = File.OpenRead(gettingStartedPat1File))
            {
                var syntaxTree = parser.Parse(stream);
                syntaxTree.AcceptVisitor(new GettingStartedPage1Visitor(page));

            }
        }

        private void ReadGettingStartedPage2(GettingStartedPage page, string gettingStartedPart2File)
        {
            using (var stream = File.OpenRead(gettingStartedPart2File))
            {
                var syntaxTree = parser.Parse(stream);
                syntaxTree.AcceptVisitor(new GettingStartedPage2Visitor(page));

            }
        }
    }

    internal class GettingStartedPage1Visitor : DepthFirstAstVisitor
    {
        private readonly GettingStartedPage _page;

        public GettingStartedPage1Visitor(GettingStartedPage page)
        {
            _page = page;
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name == "Computer")
                _page.CodeWithoutAspect = typeDeclaration.ToString();
            base.VisitTypeDeclaration(typeDeclaration);
        }
    }
    internal class GettingStartedPage2Visitor : DepthFirstAstVisitor
    {
        private readonly GettingStartedPage _page;

        public GettingStartedPage2Visitor(GettingStartedPage page)
        {
            _page = page;
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name == "LogAttribute")
                _page.AspectCode = typeDeclaration.ToString();
            if (typeDeclaration.Name == "Computer")
                _page.CodeWithAspect = typeDeclaration.ToString();
            base.VisitTypeDeclaration(typeDeclaration);
        }


        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            _page.TestWithAspect = lambdaExpression.Body.ToString();
            base.VisitLambdaExpression(lambdaExpression);
        }
    }
}