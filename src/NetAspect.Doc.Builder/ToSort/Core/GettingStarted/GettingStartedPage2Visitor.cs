using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
    internal class GettingStartedPage2Visitor : DepthFirstAstVisitor
    {
        private readonly GettingStartedPageModel _page;

        public GettingStartedPage2Visitor(GettingStartedPageModel page)
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