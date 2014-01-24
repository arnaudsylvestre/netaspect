﻿using System;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before
{
    [TestFixture]
    public class BeforeUpdateFieldFileNameParameterTest 
    {
         [Test]
         public void CheckUpdateFieldWithFileNameAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckUpdateFieldWithFileNameAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeUpdateFieldValueFileName);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdateFieldWithFileNameAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdateFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdateFieldWithFileNameAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdateFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}