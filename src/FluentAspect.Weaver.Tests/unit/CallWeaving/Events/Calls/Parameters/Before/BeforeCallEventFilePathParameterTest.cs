using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before
{
    [TestFixture]
    public class BeforeCallEventFilePathParameterTest 
    {
         [Test]
         public void CheckCallEventWithFilePathAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallEventWithFilePathAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeCallEventValueFilePath);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithFilePathAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallEventValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallEventWithFilePathAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeCallEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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