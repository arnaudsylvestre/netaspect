using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After
{
    [TestFixture]
    public class AfterCallEventColumnNumberParameterTest 
    {
         [Test]
         public void CheckCallEventWithColumnNumberAndNoDebuggingInformation()
       {
          throw new NotImplementedException();
         }

         [Test]
         public void CheckCallEventWithColumnNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter columnNumber in method AfterCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterCallEventValueColumnNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithColumnNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterCallEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithColumnNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterCallEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}