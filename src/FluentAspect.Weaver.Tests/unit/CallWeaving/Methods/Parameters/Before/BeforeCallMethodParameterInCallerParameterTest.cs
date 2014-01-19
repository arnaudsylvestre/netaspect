using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before
{
    [TestFixture]
    public class BeforeCallMethodParameterInCallerParameterTest 
    {
         [Test]
         public void CheckCallMethodWithParameterInCallerAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //          {
          //             simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
          //                .WithParameter<int>("parameterInCaller");
          //          })
          //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
          //        .AndEnsureAssembly((assembly, actual) =>
          //            {
          //                var caller = actual.CreateCallerObject();
          //                actual.CallCallerMethod(caller);
          //                Assert.AreEqual(0, actual.Aspect.BeforeCallMethodValueParameterInCaller);
          //            })
          //        .AndLaunchTest();
         }

         [Test]
         public void CheckCallMethodWithParameterInCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeCallMethodValueParameterInCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithParameterInCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckCallMethodWithParameterInCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeCallMethodValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}