using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{
    [TestFixture]
    public class AfterMethodParameterNameParameterTest
    {
       [Test]
       public void CheckParameterNameReferenced()
       {

          throw new NotImplementedException();
       }

        [Test]
       public void CheckParameterNameBadType()
       {
           throw new NotImplementedException();
           //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
           //         .ByDefiningAssembly(simpleClassAndWeaver =>
           //        {
           //           simpleClassAndWeaver.AfterInterceptor.WithParameter<int>("parameters");
           //           simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
           //        })
           //         .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the parameters parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object[]")))
           //         .AndLaunchTest();
        }

       [Test]
       public void CheckParameterNameWithRealType()
        {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithParameter<int>("first");
                       netAspectInterceptor
                           .WithReturn();
                       myClassToWeave.AddMethod("MyMethodToWeave")
                          .WithParameter<int>("first")
                          .WithReturn()
                          .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                       var o = assembly.CreateObject("MyClassToWeave");
                       o.CallMethod("MyMethodToWeave", 12);

                       Assert.AreEqual(12, assembly.GetStaticFieldValue("MyAspectAttribute", "AfterfirstField"));

                    })
                    .AndLaunchTest();
       }
   }
}