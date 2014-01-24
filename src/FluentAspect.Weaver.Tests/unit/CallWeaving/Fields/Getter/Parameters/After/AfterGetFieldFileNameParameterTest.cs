using System;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After
{
    [TestFixture]
    public class AfterGetFieldFileNameParameterTest 
    {
         [Test]
         public void CheckGetFieldWithFileNameAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckGetFieldWithFileNameAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter fileName in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterGetFieldValueFileName);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithFileNameAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterGetFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithFileNameAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("fileName");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter fileName in method AfterGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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