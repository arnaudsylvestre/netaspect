﻿using System;
using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before
{
    [TestFixture]
    public class BeforeCallMethodFileNameParameterTest 
    {
         [Test]
         public void CheckCallMethodWithFileNameAndNoDebuggingInformation()
         {
          DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                    {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("fileName");
                    })
                    .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(0, actual.Aspect.BeforeCallMethodValueFileName);
                      })
                  .AndLaunchTest();
         }

         [Test]
         public void CheckCallMethodWithFileNameAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeCallMethodValueFileName);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithFileNameAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithFileNameAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}