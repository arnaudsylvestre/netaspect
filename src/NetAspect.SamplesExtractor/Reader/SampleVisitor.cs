using ICSharpCode.NRefactory.CSharp;

namespace NetAspect.Doc.Builder
{
    public class Sample
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }


    internal class SampleVisitor : DepthFirstAstVisitor
    {
        private Sample model;

        public SampleVisitor(Sample model)
        {
            this.model = model;
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name.EndsWith("Attribute"))
            {
                model.Name = typeDeclaration.Name;
                model.Code = typeDeclaration.ToString();
            }
            base.VisitTypeDeclaration(typeDeclaration);
        }

    }
}