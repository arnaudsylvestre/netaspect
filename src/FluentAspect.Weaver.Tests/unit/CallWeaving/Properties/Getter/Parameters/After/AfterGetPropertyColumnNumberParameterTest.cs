using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After
{
    [TestFixture]
    public class AfterGetPropertyColumnNumberParameterTest 
    {
         [Test]
         public void CheckGetPropertyWithColumnNumberAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckGetPropertyWithColumnNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter columnNumber in method AfterGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterGetPropertyValueColumnNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetPropertyWithColumnNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterGetPropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetPropertyWithColumnNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterGetPropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}