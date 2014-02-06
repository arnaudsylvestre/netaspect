using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using Mono.Cecil;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{

    [TestFixture]
   public class AfterMethodResultParameterTest
   {
      [Test]
      public void CheckResultReferenced()
      {
         DoUnit2.Test().ByDefiningAssembly(assembly =>
         {
            var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
            var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
            var netAspectInterceptor = aspect.AddAfterInterceptor();
            netAspectInterceptor
               .WithReferencedParameter<string>("result");
            netAspectInterceptor
               .WithUpdateParameter("result", "New Hello")
                .WithReturn();
            myClassToWeave.AddMethod<string>("MyMethodToWeave")
               .WithReturn("Hello")
               .WithAspect(aspect);

         })
                   .AndEnsureAssembly(assemblyP =>
                   {

                      var o = assemblyP.CreateObject("MyClassToWeave");
                      var res = o.CallMethod("MyMethodToWeave");
                      Assert.AreEqual("Hello", assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterresultField"));
                      Assert.AreEqual("New Hello", res);

                   })
                   .AndLaunchTest();
      }
      [Test]
      public void CheckResultReferencedWithObjectType()
      {
         DoUnit2.Test().ByDefiningAssembly(assembly =>
         {
            var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
            var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
            var netAspectInterceptor = aspect.AddAfterInterceptor();
            netAspectInterceptor
               .WithReferencedParameter<object>("result");
            netAspectInterceptor
                .WithReturn();
            myClassToWeave.AddMethod<string>("MyMethodToWeave")
               .WithReturn("Hello")
               .WithAspect(aspect);

         })
                   .AndEnsureAssembly(assemblyP =>
                   {

                      var o = assemblyP.CreateObject("MyClassToWeave");
                      o.CallMethod("MyMethodToWeave");
                      Assert.AreEqual("Hello", assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterresultField"));

                   })
                   .AndLaunchTest();
      }
      [Test]
      public void CheckResult()
      {
         DoUnit2.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               var netAspectInterceptor = aspect.AddAfterInterceptor();
               netAspectInterceptor
                  .WithParameter<string>("result");
               netAspectInterceptor
                   .WithReturn();
               myClassToWeave.AddMethod<string>("MyMethodToWeave")
                  .WithReturn("Hello")
                  .WithAspect(aspect);

           })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.CallMethod("MyMethodToWeave");
                       Assert.AreEqual("Hello", assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterresultField"));

                   })
                   .AndLaunchTest();
        
      }
      [Test]
      public void CheckResultWithObjectType()
      {
         DoUnit2.Test().ByDefiningAssembly(assembly =>
         {
            var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
            var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
            var netAspectInterceptor = aspect.AddAfterInterceptor();
            netAspectInterceptor
               .WithParameter<object>("result");
            netAspectInterceptor
                .WithReturn();
            myClassToWeave.AddMethod<string>("MyMethodToWeave")
               .WithReturn("Hello")
               .WithAspect(aspect);

         })
                   .AndEnsureAssembly(assemblyP =>
                   {

                      var o = assemblyP.CreateObject("MyClassToWeave");
                      o.CallMethod("MyMethodToWeave");
                      Assert.AreEqual("Hello", assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterresultField"));

                   })
                   .AndLaunchTest();
      }

      [Test]
      public void CheckResultWhenVoidMethod()
      {
         DoUnit2.Test().ByDefiningAssembly(assembly =>
         {
            var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
            var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
            var netAspectInterceptor = aspect.AddAfterInterceptor();
            netAspectInterceptor
               .WithParameter<object>("result");
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
                      Assert.AreEqual("Hello", assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterresultField"));

                   })
                   .AndLaunchTest();
      }
       
   }
}