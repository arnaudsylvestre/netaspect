using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After
{
    [TestFixture]
    public class AfterCallMethodFilePathParameterTest 
    {
         [Test]
         public void CheckCallMethodWithFilePathAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckCallMethodWithFilePathAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterCallMethodValueFilePath);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithFilePathAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<string>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithFilePathAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithReferencedParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.AfterCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}