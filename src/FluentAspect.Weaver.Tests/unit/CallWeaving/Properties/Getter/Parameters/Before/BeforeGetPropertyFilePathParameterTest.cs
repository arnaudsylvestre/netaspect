using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before
{
    [TestFixture]
    public class BeforeGetPropertyFilePathParameterTest 
    {
         [Test]
         public void CheckGetPropertyWithFilePathAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckGetPropertyWithFilePathAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method BeforeGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeGetPropertyValueFilePath);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetPropertyWithFilePathAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeGetPropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetPropertyWithFilePathAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeGetPropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeGetPropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}