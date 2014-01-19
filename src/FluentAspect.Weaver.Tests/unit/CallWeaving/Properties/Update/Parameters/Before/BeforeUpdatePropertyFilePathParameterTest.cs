using System;
using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before
{
    [TestFixture]
    public class BeforeUpdatePropertyFilePathParameterTest 
    {
         [Test]
         public void CheckUpdatePropertyWithFilePathAndNoDebuggingInformation()
         {
          DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                    {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("filePath");
                    })
                    .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(0, actual.Aspect.BeforeUpdatePropertyValueFilePath);
                      })
                  .AndLaunchTest();
         }

         [Test]
         public void CheckUpdatePropertyWithFilePathAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter filePath in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeUpdatePropertyValueFilePath);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithFilePathAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdatePropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithFilePathAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("filePath");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter filePath in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdatePropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}