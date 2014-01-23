using System;
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
                 .WithReturn()
                 .WithReferencedParameter<object>("instance");
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
                .WithReturn()
                .WithOutParameter<object>("instance");
             myClassToWeave.AddMethod("MyMethodToWeave")
                .WithReturn()
                .WithAspect(aspect);

          }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method After of the type 'A.MyAspectAttribute'")))
                   .AndLaunchTest();
           
       }

        [Test]
        public void CheckInstanceBadType()
       {
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.AfterInterceptor.WithParameter<int>("instance");
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.AfterInterceptor.WithParameter<object>("instance");
                   })
                   .AndEnsureAssembly((assemblyP, result) =>
                   {
                      var o = result.CreateObjectFromClassToWeaveType();
                      result.CallWeavedMethod(o);
                      Assert.AreEqual(o, result.Aspect.AfterInstance);

                   })
                   .AndLaunchTest();
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                    {                       
                        simpleClassAndWeaver.AfterInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
                    })
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       result.CallWeavedMethod(o);
                       Assert.AreEqual(o, result.Aspect.AfterInstance);

                    })
                    .AndLaunchTest();
        }
   }
}