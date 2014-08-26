using System.Collections.Generic;
using System.IO;
using System.Linq;
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
          //string baseDirectory = @"D:\Sources\3rdParty\fluentaspect-git\fluentaspect\";
          string baseDirectory = @"D:\Developpement\fluentaspect\";

            DocumentationFromTestExtractor extractor = new DocumentationFromTestExtractor();

            var documentationFromTest_L = extractor.ExtractDocumentationFromTests(baseDirectory + @"src\FluentAspect.Weaver.Tests\docs\Documentation");

            var possibilities = new List<Possibility>();
            BuildPossibilities(possibilities, documentationFromTest_L);

             var builder = new DocBuilder();
             foreach (var possibility in possibilities)
            {
               builder.Add(possibility);
            }
             builder.Add(from p in documentationFromTest_L.Parameters select new Parameter()
                 {
                     Description = p.Description,
                     Name = p.Name,
                     Kind = p.Kind,
                 });
             builder.Generate(baseDirectory + @"web\generated.html");
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
                Group = possibilityDescription_L.Group,
                
             });
          }

          

          foreach (var test_L in documentationFromTest_P.Tests)
          {
             if (test_L.Possibility == null)
                continue;
             var possibility_L = possibilities_P.Find(p => p.Kind == test_L.Possibility);
             possibility_L.Member = test_L.Member;
             var interceptorDescription_L = documentationFromTest_P.Interceptors.Find(p => p.Name == test_L.MethodName);
             var possibilityEvent_L = new PossibilityEvent()
             {
                Called = interceptorDescription_L.Called,
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
                if (parameterDescription_L == null)
                {
                    if (aspectParameter_L.StartsWith("caller"))
                        parameterDescription_L = documentationFromTest_P.Parameters.Find(p => p.Name == "caller + parameter name");
                    else
                   parameterDescription_L = documentationFromTest_P.Parameters.Find(p => p.Name == "parameter name");
                }
                possibilityEvent_L.Parameters.Add(new Parameter()
                {
                   Name = parameterDescription_L.Name,
                   Description = parameterDescription_L.Description
                });
             }
              possibility_L.AvailableParameters.AddRange(from p in documentationFromTest_P.Parameters select new Parameter()
                  {
                      Description = p.Description,
                      Name = p.Name,
                  });
             possibility_L.Events.Add(possibilityEvent_L);
          }
       }
    }
}