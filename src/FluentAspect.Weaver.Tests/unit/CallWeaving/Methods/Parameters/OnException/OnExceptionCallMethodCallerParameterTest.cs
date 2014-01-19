using System;
using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionCallMethodCallerParameterTest 
    {
         [Test]
         public void CheckCallMethodWithCallerAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallMethodWithCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.OnExceptionCallMethodValueCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithParameter<string>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter caller in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.OnExceptionCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddOnExceptionFieldAccess()
             //              .WithReferencedParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter caller in method OnExceptionCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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