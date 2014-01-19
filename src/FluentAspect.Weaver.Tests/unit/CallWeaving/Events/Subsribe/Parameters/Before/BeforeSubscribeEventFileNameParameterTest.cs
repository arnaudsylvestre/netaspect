using System;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before
{
    [TestFixture]
    public class BeforeSubscribeEventFileNameParameterTest 
    {
         [Test]
         public void CheckSubscribeEventWithFileNameAndNoDebuggingInformation()
         {
          DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                    {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("fileName");
                    })
                    .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(0, actual.Aspect.BeforeSubscribeEventValueFileName);
                      })
                  .AndLaunchTest();
         }

         [Test]
         public void CheckSubscribeEventWithFileNameAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeSubscribeEventValueFileName);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithFileNameAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithFileNameAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}