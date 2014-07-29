using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace NetAspect.Doc.Builder
{
    public class Sample
    {
        public string AspectCode { get; set; }
        public string CallCode { get; set; }
        public string ClassToWeaveCode { get; set; }
    }

   public class DocumentationFromTestExtractor
   {
      public DocumentationFromTest ExtractDocumentationFromTests(string directoryPath_P)
      {
         DocumentationFromTest doc = new DocumentationFromTest();
         var parser = new CSharpParser();
         var files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories);
         foreach (var file_L in files_L)
         {
            using (var stream = File.OpenRead(file_L))
            {
               var syntaxTree = parser.Parse(stream);
               var test = new TestDescription();
               doc.Tests.Add(test);
               syntaxTree.AcceptVisitor(new FindInvocationsVisitor(doc.Possibilities, doc.Parameters, test));
               
            }
         }


         return doc;
         
      }
   }



   public class DocumentationFromTest
   {
      public List<ParameterDescription> Parameters = new List<ParameterDescription>(); 
      public List<PossibilityDescription> Possibilities = new List<PossibilityDescription>();
      public List<TestDescription> Tests = new List<TestDescription>(); 
   }

   public class ParameterDescription
   {
      public string Name { get; set; }
      public string Description { get; set; }
   }


   class FindInvocationsVisitor : DepthFirstAstVisitor
    {
       private readonly List<PossibilityDescription> _possibilities;
      private readonly List<ParameterDescription> _parameters;
      private readonly TestDescription _test;

       public FindInvocationsVisitor(List<PossibilityDescription> possibilities_P, List<ParameterDescription> parameters, TestDescription test_P)
       {
          _possibilities = possibilities_P;
          _parameters = parameters;
          _test = test_P;
       }

      public override void VisitAttribute(Attribute attribute)
      {
         if (attribute.Type.ToString().Contains("PossibilityDocumentationAttribute"))
         {
            var arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
            _possibilities.Add(new PossibilityDescription()
            {
               Description = GetValue(arguments[1]),
               Kind = GetValue(arguments[0]),
               Title = GetValue(arguments[2]),
            });
         }
         base.VisitAttribute(attribute);
      }

      private static string GetValue(PrimitiveExpression expression)
      {
         return expression.LiteralValue;
      }

      public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
           _test.CallCode = lambdaExpression.Body.ToString();
            base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name.EndsWith("Attribute"))
            {
               _test.AspectCode = typeDeclaration.ToString();
            }
            if (typeDeclaration.Name == "MyInt")
            {
               _test.ClassToWeaveCode = typeDeclaration.ToString();
            }
            base.VisitTypeDeclaration(typeDeclaration);
        }
    }
}