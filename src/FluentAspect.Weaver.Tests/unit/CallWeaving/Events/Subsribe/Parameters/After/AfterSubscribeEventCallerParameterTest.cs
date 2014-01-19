using System;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.After
{
    [TestFixture]
    public class AfterSubscribeEventCallerParameterTest 
    {
         [Test]
         public void CheckSubscribeEventWithCallerAndNoDebuggingInformation()
         {
          DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                    {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithParameter<int>("caller");
                    })
                    .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(0, actual.Aspect.AfterSubscribeEventValueCaller);
                      })
                  .AndLaunchTest();
         }

         [Test]
         public void CheckSubscribeEventWithCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterSubscribeEventValueCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter caller in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter caller in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}