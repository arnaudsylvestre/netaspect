using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace NetAspect.Doc.Builder
{
    public class Sample
    {
        public string AspectCode { get; set; }
        public string CallCode { get; set; }
        public string ClassToWeaveCode { get; set; }
    }

    public class SampleExtractor
    {
        public Sample ExtractSample(Stream testContent)
         {
             var parser = new CSharpParser();
             var syntaxTree = parser.Parse(testContent);
            var sample = new Sample();
            syntaxTree.AcceptVisitor(new FindInvocationsVisitor(sample));
            return sample;
         }
    }


    class FindInvocationsVisitor : DepthFirstAstVisitor
    {
        private readonly Sample _sample;

        public FindInvocationsVisitor(Sample sample)
        {
            _sample = sample;
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            _sample.CallCode = lambdaExpression.Body.ToString();
            base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name.EndsWith("Attribute"))
            {
                _sample.AspectCode = typeDeclaration.ToString();
            }
            if (typeDeclaration.Name == "MyInt")
            {
                _sample.ClassToWeaveCode = typeDeclaration.ToString();
            }
            base.VisitTypeDeclaration(typeDeclaration);
        }
    }
}