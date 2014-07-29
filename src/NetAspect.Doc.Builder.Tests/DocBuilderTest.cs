using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NetAspect.Doc.Builder.Tests.Resources;

namespace NetAspect.Doc.Builder.Tests
{



    [TestFixture]
    public class DocBuilderTest
    {
         [Test]
         public void CheckBuild()
         {
            DocumentationFromTestExtractor extractor = new DocumentationFromTestExtractor();

            var documentationFromTest_L = extractor.ExtractDocumentationFromTests(@"D:\Sources\3rdParty\fluentaspect-git\fluentaspect\src\FluentAspect.Weaver.Tests\docs\MethodPossibilities");

            var possibilities = new List<Possibility>();
            BuildPossibilities(possibilities, documentationFromTest_L);

             var builder = new DocBuilder();
             foreach (var possibility in possibilities)
            {
               builder.Add(possibility);
            }

             builder.Generate(@"D:\Sources\3rdParty\fluentaspect-git\fluentaspect\web\generated.html");
         }

       private void BuildPossibilities(List<Possibility> possibilities_P, DocumentationFromTest documentationFromTest_P)
       {
          foreach (var possibilityDescription_L in documentationFromTest_P.Possibilities)
          {
             possibilities_P.Add(new Possibility()
             {
                Description = possibilityDescription_L.Description,
                Kind = possibilityDescription_L.Kind,
                Title = possibilityDescription_L.Title,
             });
          }

          foreach (var test_L in documentationFromTest_P.Tests)
          {
             var possibility_L = possibilities_P.Find(p => p.Kind == test_L.Possibility);
             var possibilityEvent_L = new PossibilityEvent()
             {
                Called = test_L.Called,
                Description = test_L.Description,
                MethodName = test_L.MethodName,
                Sample = new Sample()
                {
                   AspectCode = test_L.AspectCode,
                   CallCode = test_L.CallCode,
                   ClassToWeaveCode = test_L.ClassToWeaveCode,
                },
                Kind = test_L.Kind,
             };

             foreach (var aspectParameter_L in test_L.AspectParameters)
             {
                var parameterDescription_L = documentationFromTest_P.Parameters.Find(p => p.Name == aspectParameter_L);
                possibilityEvent_L.Parameters.Add(new Parameter()
                {
                   Name = parameterDescription_L.Name,
                   Description = parameterDescription_L.Description
                });
             }
             possibility_L.Events.Add(possibilityEvent_L);
          }
       }
    }
}