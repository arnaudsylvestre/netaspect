using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before
{
    [TestFixture]
    public class BeforeMethodMethodParameterTest
    {
        [Test]
        public void CheckMethodReferenced()
        {

            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddBeforeInterceptor()
                   .WithReferencedParameter<MethodInfo>("method")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            })
            .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'method' in the method Before of the type 'A.MyAspectAttribute'")))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckMethodBadType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddBeforeInterceptor()
                   .WithParameter<int>("method")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method Before of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo")))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckMethodWithRealType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddBeforeInterceptor();
                netAspectInterceptor
                   .WithParameter<MethodInfo>("method");
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
                        Assert.AreEqual("MyMethodToWeave", ((MethodInfo)assemblyP.GetStaticFieldValue("MyAspectAttribute", "BeforemethodField")).Name);

                    })
                    .AndLaunchTest();
        }
   }
}