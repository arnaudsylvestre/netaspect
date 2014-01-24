using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.After
{
    [TestFixture]
    public class AfterSubscribeEventLineNumberParameterTest 
    {
         [Test]
         public void CheckSubscribeEventWithLineNumberAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckSubscribeEventWithLineNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterSubscribeEventValueLineNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithLineNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterSubscribeEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckSubscribeEventWithLineNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method AfterSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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