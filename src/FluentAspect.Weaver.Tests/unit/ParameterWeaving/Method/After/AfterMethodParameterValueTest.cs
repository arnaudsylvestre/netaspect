using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.ParameterWeaving.Method.After
{
    [TestFixture]
   public class AfterMethodParameterValueTest
    {
       [Test]
       public void CheckReferenced()
       {
          throw new NotImplementedException();
       }


       [Test]
        public void CheckInstanceWithRealType()
        {
            DoUnit.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterParameterInterceptor();
                        netAspectInterceptor
                           .WithParameter<string>("value");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithParameter<string>("first", aspect)
                           .WithReturn();
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave", new object[] { "Hello" });

                        Assert.AreEqual("Hello", assembly.GetStaticFieldValue("MyAspectAttribute", "AfterParametervalueField"));

                    })
                    .AndLaunchTest();
        }
   }
}