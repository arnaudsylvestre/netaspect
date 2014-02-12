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
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddBeforeInterceptor()
                   .WithReferencedParameter<object>("instance")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method Before of the type 'A.MyAspectAttribute'")))
                     .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceOut()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave");
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddBeforeInterceptor()
                   .WithOutParameter<object>("instance")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method Before of the type 'A.MyAspectAttribute'")))
                     .AndLaunchTest();

        }

        [Test]
        public void CheckInstanceBadType()
        {

            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave");
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddBeforeInterceptor()
                   .WithParameter<int>("instance")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                     .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceWithObjectType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddBeforeInterceptor();
                netAspectInterceptor
                   .WithParameter<object>("instance");
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