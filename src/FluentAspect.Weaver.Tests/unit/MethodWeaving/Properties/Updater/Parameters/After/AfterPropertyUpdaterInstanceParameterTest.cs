using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After
{
    [TestFixture]
    public class AfterPropertyUpdaterInstanceParameterTest
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
               var netAspectInterceptor = aspect.AddAfterPropertySetInterceptor();
               netAspectInterceptor
                  .WithParameter("instance", myClassToWeave);
               netAspectInterceptor
                   .WithReturn();
               var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                   .WithAspect(aspect);
               propertyDefinition.AddSetMethod()
                  .WithReturn()
                  ;

           })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.UpdateProperty("MyProperty", "value");
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterPropertySetinstanceField"));

                   })
                   .AndLaunchTest();
           
        }
   }
}