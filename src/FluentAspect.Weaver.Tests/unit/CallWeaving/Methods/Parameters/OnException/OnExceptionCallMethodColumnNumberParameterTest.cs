using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionCallMethodColumnNumberParameterTest 
    {
         [Test]
         public void CheckCallMethodWithColumnNumberAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallMethodWithColumnNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter columnNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.OnExceptionCallMethodValueColumnNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithColumnNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<string>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.OnExceptionCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithColumnNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithReferencedParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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