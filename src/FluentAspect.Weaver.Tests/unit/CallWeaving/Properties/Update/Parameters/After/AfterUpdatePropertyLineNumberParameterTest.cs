using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After
{
    [TestFixture]
    public class AfterUpdatePropertyLineNumberParameterTest 
    {
         [Test]
         public void CheckUpdatePropertyWithLineNumberAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckUpdatePropertyWithLineNumberAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterUpdatePropertyValueLineNumber);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithLineNumberAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterUpdatePropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithLineNumberAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("lineNumber");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterUpdatePropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}