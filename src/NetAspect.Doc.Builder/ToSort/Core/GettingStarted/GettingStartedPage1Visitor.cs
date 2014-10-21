using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
    internal class GettingStartedPage1Visitor : DepthFirstAstVisitor
    {
        private readonly GettingStartedPageModel _page;

        public GettingStartedPage1Visitor(GettingStartedPageModel page)
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
}