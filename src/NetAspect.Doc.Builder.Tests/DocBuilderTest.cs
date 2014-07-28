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
             var sampleExtractor = new SampleExtractor();
            var sample = sampleExtractor.ExtractSample(new MemoryStream(Samples.Sample1));
             var builder = new DocBuilder();
             builder.Add(new Possibility()
             {
                 Description = "Before Method Weaving possibilities",
                 Title = "On methods",
                 Member = "method",
                     Kind = "MethodWeavingBefore",
                     Events = new List<PossibilityEvent>
                         {
                             new PossibilityEvent()
                                 {
                                     MethodName = "BeforeMethod",
                                     Called = "before the method is executed", 
                                     Description = "Before Method Weaving possibilities",
                                     Kind = "MethodWeavingBefore",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 },
                                             new Parameter()
                                                 {
                                                     Description = "The method",
                                                     Name = "method"
                                                 },
                                             new Parameter()
                                                 {
                                                     Description = "The parameters",
                                                     Name = "parameters"
                                                 },
                                             new Parameter()
                                                 {
                                                     Description = "The parameter's name",
                                                     Name = "parameter name"
                                                 },
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "AfterMethod",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnExceptionMethod",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnFinallyMethod",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 }
                         }
                 });


             builder.Add(new Possibility()
             {
                 Description = "Before Constructor Weaving possibilities",
                 Title = "On constructor",
                 Member = "constructor",
                 Kind = "ConstructorWeaving",
                 Events = new List<PossibilityEvent>
                         {
                             new PossibilityEvent()
                                 {
                                     MethodName = "BeforeConstructor",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "AfterConstructor",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnExceptionConstructor",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnFinallyConstructor",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 }
                         }
             });


             builder.Add(new Possibility()
             {
                 Description = "Before Property get Weaving possibilities",
                 Title = "On property get",
                 Member = "property",
                 Kind = "PropertyGetWeaving",
                 Events = new List<PossibilityEvent>
                         {
                             new PossibilityEvent()
                                 {
                                     MethodName = "BeforeGetProperty",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "AfterGetProperty",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnExceptionGetProperty",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnFinallyGetProperty",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 }
                         }
             });


             builder.Add(new Possibility()
             {
                 Description = "Before Property set Weaving possibilities",
                 Title = "On property set",
                 Member = "property",
                 Kind = "PropertySetWeaving",
                 Events = new List<PossibilityEvent>
                         {
                             new PossibilityEvent()
                                 {
                                     MethodName = "BeforePropertySet",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "AfterPropertySet",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnExceptionPropertySet",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 },
                                 new PossibilityEvent()
                                 {
                                     MethodName = "OnFinallyPropertySet",
                                     Called = "Called", 
                                     Description = "Description",
                                     Kind = "Kind",
                                     Parameters = new List<Parameter>
                                         {
                                             new Parameter()
                                                 {
                                                     Description = "The instance",
                                                     Name = "instance"
                                                 }
                                         },
                                         Samples = new List<Sample>
                                             {
                                                 sample
                                             }
                                 }
                         }
             });

             builder.Generate(@"D:\Developpement\fluentaspect\web\generated.html");
         }
    }
}