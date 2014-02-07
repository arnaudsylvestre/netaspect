using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before
{
    [TestFixture]
    public class BeforeMethodInstanceParameterTest
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
             var netAspectInterceptor = aspect.AddBeforeInterceptor();
             netAspectInterceptor
                .WithParameter("instance", myClassToWeave);
             netAspectInterceptor
                 .WithReturn();
             myClassToWeave.AddMethod("MyMethodToWeave")
                .WithReturn()
                .WithAspect(aspect);

          })
                  .AndEnsureAssembly(assemblyP =>
                  {

                     var o = assemblyP.CreateObject("MyClassToWeave");
                     o.CallMethod("MyMethodToWeave");
                     Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "BeforeinstanceField"));

                  })
                  .AndLaunchTest();
           
        }
   }
}