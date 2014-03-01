using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After
{
    [TestFixture]
    public class AfterCallEventCallerParameterTest 
    {
         [Test]
         public void CheckCallEventWithCallerObject()
       {
             throw new NotImplementedException();
           //DoUnit.Test().ByDefiningAssembly(assembly =>
           //{
           //    var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
           //    var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
           //    var netAspectInterceptor = aspect.AddAfterCallEventInterceptor();
           //    netAspectInterceptor
           //       .WithParameter<object>("caller");
           //    netAspectInterceptor
           //        .WithReturn();
           //    var eventDefinition = myClassToWeave.AddEvent<Action>("MyEvent")
           //        .WithAspect(aspect)
           //        ;
           //    myClassToWeave.AddMethod("RaiseEvent")
           //        .WhichCallEvent<Action>(eventDefinition)
           //        .WithReturn()
           //        ;

           //})
           //         .AndEnsureAssembly(assemblyP =>
           //         {

           //             var o = assemblyP.CreateObject("MyClassToWeave");
           //             o.GetType().GetEvent("MyEvent").AddEventHandler(o, new Action(() => { }));
           //             o.CallMethod("RaiseEvent");
           //             Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterCallEventcallerField"));

           //         })
           //         .AndLaunchTest();
          
         }

         [Test]
         public void CheckCallEventWithCallerRealType()
         {
             throw new NotImplementedException();
             //DoUnit.Test().ByDefiningAssembly(assembly =>
             //{
             //    var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
             //    var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
             //    var netAspectInterceptor = aspect.AddAfterCallEventInterceptor();
             //    netAspectInterceptor
             //       .WithParameter("caller", myClassToWeave);
             //    netAspectInterceptor
             //        .WithReturn();
             //    var eventDefinition = myClassToWeave.AddEvent<Action>("MyEvent")
             //        .WithAspect(aspect)
             //        ;
             //    myClassToWeave.AddMethod("RaiseEvent")
             //        .WhichCallEvent<Action>(eventDefinition)
             //        .WithReturn()
             //        ;

             //})
             //       .AndEnsureAssembly(assemblyP =>
             //       {

             //           var o = assemblyP.CreateObject("MyClassToWeave");
             //           o.GetType().GetEvent("MyEvent").AddEventHandler(o, new Action(() => { }));
             //           o.CallMethod("RaiseEvent");
             //           Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterCallEventcallerField"));

             //       })
             //       .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithCallerWithBadlType()
         {

             throw new NotImplementedException();
             //DoUnit.Test().ByDefiningAssembly(assembly =>
             //{
             //    var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
             //    var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
             //    var netAspectInterceptor = aspect.AddAfterCallEventInterceptor();
             //    netAspectInterceptor
             //       .WithParameter<int>("caller");
             //    netAspectInterceptor
             //        .WithReturn();
             //    var eventDefinition = myClassToWeave.AddEvent<Action>("MyEvent")
             //        .WithAspect(aspect)
             //        ;
             //    myClassToWeave.AddMethod("RaiseEvent")
             //        .WhichCallEvent<Action>(eventDefinition)
             //        .WithReturn()
             //        ;

             //})
             //.EnsureErrorHandler(e => e.Errors.Add(""))
             //       .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithCallerRealTypeReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test().ByDefiningAssembly(assembly =>
             //{
             //    var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
             //    var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
             //    var netAspectInterceptor = aspect.AddAfterCallEventInterceptor();
             //    netAspectInterceptor
             //       .WithReferencedParameter<object>("caller");
             //    netAspectInterceptor
             //        .WithReturn();
             //    var eventDefinition = myClassToWeave.AddEvent<Action>("MyEvent")
             //        .WithAspect(aspect)
             //        ;
             //    myClassToWeave.AddMethod("RaiseEvent")
             //        .WhichCallEvent<Action>(eventDefinition)
             //        .WithReturn()
             //        ;

             //})
             //.EnsureErrorHandler(e => e.Errors.Add(""))
             //       .AndLaunchTest();
         }

    }
}