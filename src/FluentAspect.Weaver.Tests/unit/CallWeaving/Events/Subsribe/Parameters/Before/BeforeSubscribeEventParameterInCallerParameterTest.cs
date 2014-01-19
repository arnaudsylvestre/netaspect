using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before
{
    [TestFixture]
    public class BeforeSubscribeEventParameterInCallerParameterTest 
    {
         [Test]
         public void CheckSubscribeEventWithParameterInCallerAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //          {
          //             simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
          //                .WithParameter<int>("parameterInCaller");
          //          })
          //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
          //        .AndEnsureAssembly((assembly, actual) =>
          //            {
          //                var caller = actual.CreateCallerObject();
          //                actual.CallCallerMethod(caller);
          //                Assert.AreEqual(0, actual.Aspect.BeforeSubscribeEventValueParameterInCaller);
          //            })
          //        .AndLaunchTest();
         }

         [Test]
         public void CheckSubscribeEventWithParameterInCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeSubscribeEventValueParameterInCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithParameterInCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithParameterInCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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