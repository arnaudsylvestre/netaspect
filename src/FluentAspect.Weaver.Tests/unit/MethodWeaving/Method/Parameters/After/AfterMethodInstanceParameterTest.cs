using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using Mono.Cecil;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{

    [TestFixture]
   public class AfterMethodInstanceParameterTest
   {
       [Test]
       public void CheckInstanceReferenced()
       {
          DoUnit2.Test().ByDefiningAssembly(assembly =>
             {
              var myClassToWeave = assembly.AddClass("MyClassToWeave");
              var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
              aspect.AddAfterInterceptor()
                 .WithReferencedParameter<object>("instance")
                 .WithReturn();
              myClassToWeave.AddMethod("MyMethodToWeave")
                 .WithReturn()
                 .WithAspect(aspect);
                
             }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method After of the type 'A.MyAspectAttribute'")))
                   .AndLaunchTest();
       }

       [Test]
       public void CheckInstanceOut()
       {
          DoUnit2.Test().ByDefiningAssembly(assembly =>
          {
             var myClassToWeave = assembly.AddClass("MyClassToWeave");
             var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
             aspect.AddAfterInterceptor()
                .WithOutParameter<object>("instance")
                .WithReturn();
             myClassToWeave.AddMethod("MyMethodToWeave")
                .WithReturn()
                .WithAspect(aspect);

          }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method After of the type 'A.MyAspectAttribute'")))
                   .AndLaunchTest();
           
       }

        [Test]
        public void CheckInstanceBadType()
       {

           DoUnit2.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave");
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               aspect.AddAfterInterceptor()
                  .WithParameter<int>("instance")
                  .WithReturn();
               myClassToWeave.AddMethod("MyMethodToWeave")
                  .WithReturn()
                  .WithAspect(aspect);

           }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterInterceptor();
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
                           Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterinstanceField"));

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
               var netAspectInterceptor = aspect.AddAfterInterceptor();
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
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterinstanceField"));

                   })
                   .AndLaunchTest();
        }
   }
}