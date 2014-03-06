using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Multi
{
    [TestFixture]
    public class MultiCallWeavingTest 
    {
         [Test]
         public void CheckCallFrom2DifferentInterceptors()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //          {
          //             simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
          //                .WithParameter<int>("caller");
          //          })
          //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
          //        .AndEnsureAssembly((assembly, actual) =>
          //            {
          //                var caller = actual.CreateCallerObject();
          //                actual.CallCallerMethod(caller);
          //                Assert.AreEqual(0, actual.Aspect.AfterCallMethodValueCaller);
          //            })
          //        .AndLaunchTest();
        }

         [Test]
         public void CheckCallsOn2DifferentCallMethodButWeavedInSameMethodTest()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterCallMethodValueCaller);
             //        })
             //        .AndLaunchTest();
         }

         [Test]
         public void CheckCallWeavingAndMethodWeavingOnsameMethod()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterCallMethodValueCaller);
             //        })
             //        .AndLaunchTest();
         }

         [Test]
         public void CheckCallWeavingInWeavedMethod()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
             //              .WithParameter<int>("caller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter caller in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.AfterCallMethodValueCaller);
             //        })
             //        .AndLaunchTest();
         }      



    }
}