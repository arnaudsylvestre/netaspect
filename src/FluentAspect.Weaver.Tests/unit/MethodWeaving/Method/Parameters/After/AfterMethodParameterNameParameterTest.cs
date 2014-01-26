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
                            .WithReferencedParameter<string>("first")
                            .WithUpdateParameter("first", "intercepted")
                             .WithReturn();
                         myClassToWeave.AddMethod<string>("MyMethodToWeave")
                            .WithReferencedParameter<string>("first")
                            .WithReturnParameter("first")
                            .WithAspect(aspect);
                     })
                     .AndEnsureAssembly(assembly =>
                     {
                         var o = assembly.CreateObject("MyClassToWeave");
                         var parameters = new object[] {"Hello"};
                         o.CallMethod("MyMethodToWeave", parameters);

                         Assert.AreEqual("intercepted", parameters[0]);

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
       public void CheckParameterNameWithObjectType()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<object>("first");
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
           DoUnit2.Test()
                   .ByDefiningAssembly(assembly =>
                   {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithParameter<object>("first");
                       netAspectInterceptor
                           .WithReturn();
                       myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithGenericType("T")
                          .WithGenericParameter("first", "T")
                          .WithReturn()
                          .WithAspect(aspect);
                   })
                   .AndEnsureAssembly(assembly =>
                   {
                       var o = assembly.CreateObject("MyClassToWeave");
                       o.CallMethod<int>("MyMethodToWeave", 12);

                       Assert.AreEqual(12, assembly.GetStaticFieldValue("MyAspectAttribute", "AfterfirstField"));

                   })
                   .AndLaunchTest();
       }

       [Test]
       public void CheckParameterNameWithGenericParameterObject()
       {
           DoUnit2.Test()
                   .ByDefiningAssembly(assembly =>
                   {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithParameter<object>("first");
                       netAspectInterceptor
                           .WithReturn();
                       myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithGenericType("T")
                          .WithGenericParameter("first", "T")
                          .WithReturn()
                          .WithAspect(aspect);
                   })
                   .AndEnsureAssembly(assembly =>
                   {
                       var o = assembly.CreateObject("MyClassToWeave");
                       o.CallMethod<string>("MyMethodToWeave", "12");

                       Assert.AreEqual("12", assembly.GetStaticFieldValue("MyAspectAttribute", "AfterfirstField"));

                   })
                   .AndLaunchTest();
       }

       [Test]
       public void CheckParameterNameWithGenericParameterAndRefObject()
       {
           DoUnit2.Test()
                   .ByDefiningAssembly(assembly =>
                   {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithReferencedParameter<object>("first");
                       netAspectInterceptor
                           .WithReturn();
                       myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithGenericType("T")
                          .WithGenericParameter("first", "T")
                          .WithReturn()
                          .WithAspect(aspect);
                   })
                   .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("Impossible to ref a generic parameter")))
                    
                   .AndLaunchTest();
       }

       [Test]
       public void CheckParameterNameWithRefGenericParameterAndObject()
       {
           DoUnit2.Test()
                   .ByDefiningAssembly(assembly =>
                   {
                       var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                       var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                       var netAspectInterceptor = aspect.AddAfterInterceptor();
                       netAspectInterceptor
                          .WithParameter<object>("first");
                       netAspectInterceptor
                           .WithReturn();
                       myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithGenericType("T")
                          .WithReferencedGenericParameter("first", "T")
                          .WithReturn()
                          .WithAspect(aspect);
                   })
                   .AndEnsureAssembly(assembly =>
                   {
                       var o = assembly.CreateObject("MyClassToWeave");
                       o.CallMethod<string>("MyMethodToWeave", new object[]{"12"});

                       Assert.AreEqual("12", assembly.GetStaticFieldValue("MyAspectAttribute", "AfterfirstField"));

                   })

                   .AndLaunchTest();
       }

       [Test]
       public void CheckValueType()
       {
           throw new NotImplementedException();
       }

       [Test]
       public void CheckValueTypeByReferenceInMethod()
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
                           .WithReferencedParameter<int>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] {12};
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void CheckValueTypeByReferenceInMethodAndReferenceInInterceptor()
       {
           throw new NotImplementedException();

       }

       [Test]
       public void CheckValueTypeReferencedInInterceptor()
       {
           throw new NotImplementedException();

       }
    }
}