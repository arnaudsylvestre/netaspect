using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Core.Readers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.GettingStarted
{
    internal class GettingStartedSection1Visitor : DepthFirstAstVisitor, ICsFileReaderVisitor<GettingStartedPageModel>
    {
        private GettingStartedPageModel _page;

        public void SetModel(GettingStartedPageModel page)
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