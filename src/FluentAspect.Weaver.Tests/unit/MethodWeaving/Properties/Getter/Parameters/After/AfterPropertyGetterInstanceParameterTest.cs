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
            throw new NotImplementedException();
          
       }

        [Test]
        public void CheckInstanceBadType()
       {
           throw new NotImplementedException();
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            throw new NotImplementedException();
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           DoUnit2.Test().ByDefiningAssembly(assembly =>
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