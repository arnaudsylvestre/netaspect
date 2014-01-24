using System;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After
{
    [TestFixture]
    public class AfterGetFieldColumnNumberParameterTest 
    {
         [Test]
         public void CheckGetFieldWithColumnNumberAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckGetFieldWithColumnNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter columnNumber in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterGetFieldValueColumnNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithColumnNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterGetFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithColumnNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("columnNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter columnNumber in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterGetFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}