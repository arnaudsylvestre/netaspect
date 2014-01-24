using System;
using FluentAspect.Weaver.Tests.acceptance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before
{
    [TestFixture]
    public class BeforeGetFieldFilePathParameterTest 
    {
         [Test]
         public void CheckGetFieldWithFilePathAndNoDebuggingInformation()
         {

            throw new NotImplementedException();
         }

         [Test]
         public void CheckGetFieldWithFilePathAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method BeforeGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeGetFieldValueFilePath);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithFilePathAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeGetFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckGetFieldWithFilePathAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeGetFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeGetFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}