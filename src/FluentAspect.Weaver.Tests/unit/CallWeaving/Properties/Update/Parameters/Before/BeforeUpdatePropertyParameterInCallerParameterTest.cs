using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before
{
    [TestFixture]
    public class BeforeUpdatePropertyParameterInCallerParameterTest 
    {
         [Test]
         public void CheckUpdatePropertyWithParameterInCallerAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //          {
          //             simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
          //                .WithParameter<int>("parameterInCaller");
          //          })
          //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
          //        .AndEnsureAssembly((assembly, actual) =>
          //            {
          //                var caller = actual.CreateCallerObject();
          //                actual.CallCallerMethod(caller);
          //                Assert.AreEqual(0, actual.Aspect.BeforeUpdatePropertyValueParameterInCaller);
          //            })
          //        .AndLaunchTest();
         }

         [Test]
         public void CheckUpdatePropertyWithParameterInCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeUpdatePropertyValueParameterInCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithParameterInCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdatePropertyValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdatePropertyWithParameterInCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
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