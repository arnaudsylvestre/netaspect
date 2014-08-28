using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Helpers;
using NetAspect.Doc.Builder.Model;

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


      public WeavingModel ExtractWeaving(string directoryPath_P)
      {
         WeavingModel doc = new WeavingModel();
         var parser = new CSharpParser();
         using (var stream = File.OpenRead(Path.Combine(directoryPath_P, "WeaveWithAttributeSampleTest.cs")))
         {
            var syntaxTree = parser.Parse(stream);
            var test = new InterceptorDocumentation();
            syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
            doc.WeaveWithAttributeSampleAspect = test.AspectCode;
            doc.WeaveWithAttributeSampleClassToWeave = test.ClassToWeaveCode;
         }
         using (var stream = File.OpenRead(Path.Combine(directoryPath_P, "WeaveWithSelectSampleTest.cs")))
         {
            var syntaxTree = parser.Parse(stream);
            var test = new InterceptorDocumentation();
            syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
            if (test.Name != null)
               doc.WeaveWithSelectSampleAspect = test.AspectCode;
            doc.WeaveWithSelectSampleClassToWeave = test.ClassToWeaveCode;
         }

         return doc;
      }

      public List<InterceptorDocumentation> ExtractInterceptors(string directoryPath_P)
       {
           List<InterceptorDocumentation> doc = new List<InterceptorDocumentation>();
           var parser = new CSharpParser();
           var files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories).OrderBy(Path.GetFileName);
           foreach (var file_L in files_L)
           {
               using (var stream = File.OpenRead(file_L))
               {
                   var syntaxTree = parser.Parse(stream);
                   var test = new InterceptorDocumentation();
                   syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                   if(test.Name != null)
                   doc.Add(test);

               }
           }


           return doc;
       }

      public DocumentationFromTest ExtractDocumentationFromTests(string directoryPath_P)
      {
         DocumentationFromTest doc = new DocumentationFromTest();
         var parser = new CSharpParser();
         var files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories).OrderBy(f => Path.GetFileName(f));
         foreach (var file_L in files_L)
         {
            using (var stream = File.OpenRead(file_L))
            {
               var syntaxTree = parser.Parse(stream);
               var test = new TestDescription();
               doc.Tests.Add(test);
               syntaxTree.AcceptVisitor(new FindInvocationsVisitor(doc.Possibilities, doc.Parameters, test, doc.Interceptors));
               
            }
         }


         return doc;
         
      }

      public List<ParameterModel> ExtractParameters(string directoryPath_P)
      {
         List<ParameterModel> doc = new List<ParameterModel>();
         var parser = new CSharpParser();
         var files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories).OrderBy(Path.GetFileName);
         foreach (var file_L in files_L)
         {
            using (var stream = File.OpenRead(file_L))
            {
               var syntaxTree = parser.Parse(stream);
               var test = new InterceptorDocumentation();
               syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                  doc.Add(new ParameterModel()
                  {
                     Samples = 
                  });

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
      public List<InterceptorDescription> Interceptors = new List<InterceptorDescription>();

   }

   public class ParameterDescription
   {
      public string Name { get; set; }
      public string Description { get; set; }

       public string Kind { get; set; }
   }


   class FindInvocationsVisitor : DepthFirstAstVisitor
   {
      private readonly List<PossibilityDescription> _possibilities;
      private readonly List<InterceptorDescription> _interceptors;
       private readonly List<ParameterDescription> _parameters;
      private readonly TestDescription _test;

       public FindInvocationsVisitor(List<PossibilityDescription> possibilities_P, List<ParameterDescription> parameters, TestDescription test_P, List<InterceptorDescription> interceptors_P)
       {
          _possibilities = possibilities_P;
          _parameters = parameters;
          _test = test_P;
          _interceptors = interceptors_P;
       }

      public override void VisitAttribute(Attribute attribute)
       {
           if (attribute.Type.ToString().Contains("PossibilityDocumentation"))
           {
               var arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
               var group = GetValue(arguments[3]);
               var possibilityDescription = new PossibilityDescription()
                   {
                       Description = GetValue(arguments[1]), Kind = GetValue(arguments[0]), Title = GetValue(arguments[2]),
                       Group = group
                   };
                       _possibilities.Add(possibilityDescription);
               
           }
          if (attribute.Type.ToString().Contains("ParameterDescription"))
          {
             var arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
             _parameters.Add(new ParameterDescription()
             {
                Name = GetValue(arguments[0]),
                Description = GetValue(arguments[1]),
                Kind = GetValue(arguments[2]),
             });
          }
          if (attribute.Type.ToString().Contains("InterceptorDescription"))
          {
             var arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
             _interceptors.Add(new InterceptorDescription()
             {
                Name = GetValue(arguments[0]),
                Called = GetValue(arguments[1]),
             });
          }
         base.VisitAttribute(attribute);
      }

      private static string GetValue(PrimitiveExpression expression)
      {
         return expression.LiteralValue.Replace("\"", "");
      }

      public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
      {
          if (IsWeaved(parameterDeclaration.Attributes))
              _test.Member = "parameter";
          base.VisitParameterDeclaration(parameterDeclaration);
      }

      public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
      {

          if (IsWeaved(propertyDeclaration.Attributes))
              _test.Member = "property";
          base.VisitPropertyDeclaration(propertyDeclaration);
      }

      public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
      {
          if (IsWeaved(fieldDeclaration.Attributes))
              _test.Member = "field";
          base.VisitFieldDeclaration(fieldDeclaration);
      }

      public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
      {
         if (constructorDeclaration.Initializer.Arguments.Count == 3)
         {
            var arguments = constructorDeclaration.Initializer.Arguments.Cast<PrimitiveExpression>().ToList();
               _test.Kind = GetValue(arguments[1]);
               _test.Possibility = GetValue(arguments[2]);
               _test.Description = GetValue(arguments[0]);
         }

         if (IsWeaved(constructorDeclaration.Attributes))
             _test.Member = "constructor";
         base.VisitConstructorDeclaration(constructorDeclaration);
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

      public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
      {
         if (((TypeDeclaration)methodDeclaration.Parent).Name.EndsWith("Attribute"))
         {
            _test.MethodName = methodDeclaration.Name;
            foreach (var parameter_L in methodDeclaration.Parameters)
            {
               _test.AspectParameters.Add(parameter_L.Name);
               
            }
         }
         if (IsWeaved(methodDeclaration.Attributes))
            _test.Member = "method";
         base.VisitMethodDeclaration(methodDeclaration);
      }

      private bool IsWeaved(AstNodeCollection<AttributeSection> attributes_P)
      {
         return attributes_P.SelectMany(attributeSection_L => attributeSection_L.Attributes).Any(attribute_L => ((SimpleType) attribute_L.Type).Identifier == "Log");
      }
    }

   public class InterceptorDescription
   {
      public string Name { get; set; }
      public string Called { get; set; }

   }



   class InterceptorDocumentationVisitor : DepthFirstAstVisitor
   {
       private readonly InterceptorDocumentation model;

       public InterceptorDocumentationVisitor(InterceptorDocumentation model)
       {
           this.model = model;
       }


       public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
       {
           if (IsWeaved(parameterDeclaration.Attributes))
               model.Member = "parameter";
           base.VisitParameterDeclaration(parameterDeclaration);
       }

       public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
       {

           if (IsWeaved(propertyDeclaration.Attributes))
               model.Member = "property";
           base.VisitPropertyDeclaration(propertyDeclaration);
       }

       public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
       {
           if (IsWeaved(fieldDeclaration.Attributes))
               model.Member = "field";
           base.VisitFieldDeclaration(fieldDeclaration);
       }

       public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
       {
           if (constructorDeclaration.Initializer.Arguments.Count == 3)
           {
               var arguments = constructorDeclaration.Initializer.Arguments.Cast<PrimitiveExpression>().ToList();
               model.When = GetValue(arguments[0]);
           }
           if (IsWeaved(constructorDeclaration.Attributes))
               model.Member = "constructor";
           base.VisitConstructorDeclaration(constructorDeclaration);
       }


       private static string GetValue(PrimitiveExpression expression)
       {
           return expression.LiteralValue.Replace("\"", "");
       }

       public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
       {
           model.CallCode = lambdaExpression.Body.ToString();
           base.VisitLambdaExpression(lambdaExpression);
       }

       public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
       {
           if (typeDeclaration.Name.EndsWith("Attribute"))
           {
               model.AspectCode = typeDeclaration.ToString();
           }
           if (typeDeclaration.Name == "MyInt")
           {
               model.ClassToWeaveCode = typeDeclaration.ToString();
           }
           base.VisitTypeDeclaration(typeDeclaration);
       }

       public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
       {
           if (((TypeDeclaration)methodDeclaration.Parent).Name.EndsWith("Attribute"))
           {
               model.Name = methodDeclaration.Name;
               foreach (var parameter_L in methodDeclaration.Parameters)
               {
                   model.Parameters.Add(parameter_L.Name);

               }
           }
           if (IsWeaved(methodDeclaration.Attributes))
               model.Member = "method";
           base.VisitMethodDeclaration(methodDeclaration);
       }

       private bool IsWeaved(AstNodeCollection<AttributeSection> attributes_P)
       {
           return attributes_P.SelectMany(attributeSection_L => attributeSection_L.Attributes).Any(attribute_L => ((SimpleType)attribute_L.Type).Identifier == "Log");
       }
   }

}