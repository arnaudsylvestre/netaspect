using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After
{
    [TestFixture]
    public class AfterUpdateFieldCallerParameterTest 
    {
        [Test]
        public void CheckCallEventWithCallerObject()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterUpdateFieldInterceptor();
                netAspectInterceptor
                   .WithParameter<object>("caller");
                netAspectInterceptor
                    .WithReturn();
                var fieldDefinition = myClassToWeave.AddField<int>("field")
                    .WithAspect(aspect)
                    ;
                myClassToWeave.AddMethod("UpdateField")
                    .WhichAffectField(fieldDefinition, 8)
                    .WithReturn()
                    ;

            })
                     .AndEnsureAssembly(assemblyP =>
                     {

                         var o = assemblyP.CreateObject("MyClassToWeave");
                         o.GetType().GetEvent("MyEvent").AddEventHandler(o, new Action(() => { }));
                         o.CallMethod("UpdateField");
                         Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterUpdateFieldcallerField"));

                     })
                     .AndLaunchTest();

        }

        [Test]
        public void CheckCallEventWithCallerRealType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterUpdateFieldInterceptor();
                netAspectInterceptor
                   .WithParameter("caller", myClassToWeave);
                netAspectInterceptor
                    .WithReturn();
                var fieldDefinition = myClassToWeave.AddField<int>("field")
                    .WithAspect(aspect)
                    ;
                myClassToWeave.AddMethod("UpdateField")
                    .WhichAffectField(fieldDefinition, 8)
                    .WithReturn()
                    ;

            })
                     .AndEnsureAssembly(assemblyP =>
                     {

                         var o = assemblyP.CreateObject("MyClassToWeave");
                         o.GetType().GetEvent("MyEvent").AddEventHandler(o, new Action(() => { }));
                         o.CallMethod("UpdateField");
                         Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterUpdateFieldcallerField"));

                     })
                     .AndLaunchTest();
        }


        [Test]
        public void CheckCallEventWithCallerWithBadlType()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void CheckCallEventWithCallerRealTypeReferenced()
        {

            throw new NotImplementedException();
        }

    }
}