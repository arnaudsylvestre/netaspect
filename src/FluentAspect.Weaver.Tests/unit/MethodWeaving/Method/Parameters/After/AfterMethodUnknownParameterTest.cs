using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{
    [TestFixture]
    public class AfterMethodUnknownParameterTest
    {
        [Test]
         public void CheckUnknown()
         {
             DoUnit2.Test().ByDefiningAssembly(assembly =>
             {
                 var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                 var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                 aspect.AddAfterInterceptor()
                    .WithParameter<int>("unknownParameter")
                    .WithReturn();
                 myClassToWeave.AddMethod("MyMethodToWeave")
                    .WithReturn()
                    .WithAspect(aspect);

             })
           .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("The parameter unknownParameter is not supported")))
                   .AndLaunchTest();
         }

        [Test]
        public void CheckWithParameterNameSameAsKeyword()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddAfterInterceptor()
                   .WithParameter("instance", myClassToWeave)
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                    .WithParameter<int>("instance")
                   .WithReturn()
                   .WithAspect(aspect);

            })
           .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("The parameter instance is already declared")))
                   .AndLaunchTest();
        }
    }
}