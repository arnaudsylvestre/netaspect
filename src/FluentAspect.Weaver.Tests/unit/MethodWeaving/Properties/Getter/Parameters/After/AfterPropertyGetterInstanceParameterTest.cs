using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After
{
    [TestFixture]
   public class AfterPropertyGetterInstanceParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
        {
            DoUnit.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterPropertyGetInterceptor();
                netAspectInterceptor
                   .WithReferencedParameter<object>("instance");
                netAspectInterceptor
                    .WithReturn();
                var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                    .WithAspect(aspect);
                propertyDefinition.AddGetMethod()
                   .WithReturn("Value")
                   ;

            }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method AfterPropertyGet of the type 'A.MyAspectAttribute'")))
                     .AndLaunchTest();
          
       }

        [Test]
        public void CheckInstanceBadType()
       {
           DoUnit.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               var netAspectInterceptor = aspect.AddAfterPropertyGetInterceptor();
               netAspectInterceptor
                  .WithParameter<int>("instance");
               netAspectInterceptor
                   .WithReturn();
               var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                   .WithAspect(aspect);
               propertyDefinition.AddGetMethod()
                  .WithReturn("Value")
                  ;

           }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method AfterPropertyGet of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            DoUnit.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterPropertyGetInterceptor();
                netAspectInterceptor
                   .WithParameter<object>("instance");
                netAspectInterceptor
                    .WithReturn();
                var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                    .WithAspect(aspect);
                propertyDefinition.AddGetMethod()
                   .WithReturn("Value")
                   ;

            })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.CallGetProperty("MyProperty");
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterPropertyGetinstanceField"));

                   })
                   .AndLaunchTest();
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           DoUnit.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               var netAspectInterceptor = aspect.AddAfterPropertyGetInterceptor();
               netAspectInterceptor
                  .WithParameter("instance", myClassToWeave);
               netAspectInterceptor
                   .WithReturn();
               var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                   .WithAspect(aspect);
               propertyDefinition.AddGetMethod()
                  .WithReturn("Value")
                  ;

           })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.CallGetProperty("MyProperty");
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterPropertyGetinstanceField"));

                   })
                   .AndLaunchTest();
        }
   }
}