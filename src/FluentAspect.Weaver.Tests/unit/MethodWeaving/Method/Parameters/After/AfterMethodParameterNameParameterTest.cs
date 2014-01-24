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

          //throw new NotImplementedException();

          DoUnit2.Test()
                   .ByDefiningAssembly(assembly =>
                   {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithReferencedParameter<int>("first")
                          .WithUpdateParameter("first", 2)
                           .WithReturn();
                       myClassToWeave.AddMethod<int>("MyMethodToWeave")
                          .WithParameter<int>("first")
                          .WithReturnParameter("first")
                          .WithAspect(aspect);
                   })
                   .AndEnsureAssembly(assembly =>
                   {
                       var o = assembly.CreateObject("MyClassToWeave");
                       o.CallMethod("MyMethodToWeave", 12);

                       Assert.AreEqual(2, assembly.GetStaticFieldValue("MyAspectAttribute", "AfterfirstField"));

                   })
                   .AndLaunchTest();
       }

        [Test]
       public void CheckParameterNameBadType()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<string>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithParameter<int>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the first parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.String' but it is expected to be System.Int32 because of the type of this parameter in the method MyMethodToWeave of the type A.MyClassToWeave")))
                    .AndLaunchTest();
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

        [Test]
        public void CheckParameterNameWithGenericParameter()
        {
            throw new NotImplementedException();
        }
    }
}