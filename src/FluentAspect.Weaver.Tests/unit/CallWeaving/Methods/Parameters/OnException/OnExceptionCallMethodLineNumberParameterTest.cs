using System;
using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionCallMethodLineNumberParameterTest 
    {
         [Test]
         public void CheckCallMethodWithLineNumberAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallMethodWithLineNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.OnExceptionCallMethodValueLineNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithLineNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<string>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.OnExceptionCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithLineNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithReferencedParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.OnExceptionCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}