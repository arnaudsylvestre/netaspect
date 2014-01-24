using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before
{
    [TestFixture]
    public class BeforeCallEventLineNumberParameterTest 
    {
         [Test]
         public void CheckCallEventWithLineNumberAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallEventWithLineNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeCallEventValueLineNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithLineNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithLineNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}