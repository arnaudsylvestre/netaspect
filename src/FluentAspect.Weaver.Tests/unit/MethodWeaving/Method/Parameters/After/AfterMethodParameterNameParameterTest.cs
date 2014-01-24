using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{
    [TestFixture]
    public class AfterMethodParameterNameParameterTest
    {
       [Test]
       public void CheckParameterNameReferenced()
       {

          throw new NotImplementedException();
       }

        [Test]
        public void CheckParametersBadType()
       {
           throw new NotImplementedException();
           //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
           //         .ByDefiningAssembly(simpleClassAndWeaver =>
           //        {
           //           simpleClassAndWeaver.AfterInterceptor.WithParameter<int>("parameters");
           //           simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
           //        })
           //         .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the parameters parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object[]")))
           //         .AndLaunchTest();
        }

       [Test]
       public void CheckParametersWithRealType()
        {
          //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
          //         .ByDefiningAssembly(simpleClassAndWeaver =>
          //         {
          //            simpleClassAndWeaver.AfterInterceptor.WithParameter<object[]>("parameters");
          //            simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
          //         })
          //         .AndEnsureAssembly((assemblyP, result) =>
          //         {
          //            var o = result.CreateObjectFromClassToWeaveType();
          //            result.CallWeavedMethod(o, 12);
          //            Assert.AreEqual(new object[]
          //               {
          //                  12
          //               }, result.Aspect.AfterParameters);

          //         })
          //         .AndLaunchTest();
       }
   }
}