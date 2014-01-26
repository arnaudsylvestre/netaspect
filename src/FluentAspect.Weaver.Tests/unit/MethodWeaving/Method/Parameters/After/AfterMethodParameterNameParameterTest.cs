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
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }
       [Test]
       public void CheckShortByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<short>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<short>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void CheckByteByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<byte>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<byte>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void CheckUshortByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<ushort>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<ushort>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void ChecksbyteByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<sbyte>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<sbyte>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void Checkuint32ByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<UInt32>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<UInt32>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void Checkuint64ByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<UInt64>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<UInt64>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void Checkint64ByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<Int64>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<Int64>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }

        public enum TestEnum
        {
            A,
            B,
            C,
        }

       [Test]
       public void CheckEnumByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<TestEnum>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<TestEnum>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { TestEnum.A };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(TestEnum.A, parameters[0]);

                    })
                    .AndLaunchTest();

       }

        public struct TestStruct
        {
            public int A;
        }

        

       [Test]
       public void CheckStructByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<TestStruct>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<TestStruct>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { new TestStruct() {A = 1} };
                        o.CallMethod("MyMethodToWeave", parameters);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void CheckBoolParameterByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<bool>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<bool>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        var parameters = new object[] { true };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(true, parameters[0]);

                    })
                    .AndLaunchTest();

       }
       [Test]
       public void CheckFloatParameterByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<float>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<float>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave", 12.5f);

                    })
                    .AndLaunchTest();

       }

       [Test]
       public void CheckDoubleParameterByReferenceInMethod()
       {
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithParameter<double>("first");
                        netAspectInterceptor
                            .WithReturn();
                        myClassToWeave.AddMethod("MyMethodToWeave")
                           .WithReferencedParameter<double>("first")
                           .WithReturn()
                           .WithAspect(aspect);
                    })
                    .AndEnsureAssembly(assembly =>
                    {
                        var o = assembly.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave", 12.5d);

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
           DoUnit2.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                        var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                        var netAspectInterceptor = aspect.AddAfterInterceptor();
                        netAspectInterceptor
                           .WithReferencedParameter<int>("first");
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
                        var parameters = new object[] { 12 };
                        o.CallMethod("MyMethodToWeave", parameters);

                        Assert.AreEqual(12, parameters[0]);

                    })
                    .AndLaunchTest();

       }
    }
}