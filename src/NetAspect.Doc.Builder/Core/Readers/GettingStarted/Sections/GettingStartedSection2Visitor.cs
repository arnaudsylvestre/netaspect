using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Core.Readers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
    internal class GettingStartedSection2Visitor : DepthFirstAstVisitor, ICsFileReaderVisitor<GettingStartedPageModel>
    {
        private GettingStartedPageModel _page;

        public void SetModel(GettingStartedPageModel page)
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