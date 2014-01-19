using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    [TestFixture]
    public class BeforeMethodParameterNameParameterTest
    {
       [Test]
       public void CheckParameterNameReferenced()
       {
           throw new NotImplementedException();
          //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //        {
          //           simpleClassAndWeaver.BeforeInterceptor.WithReferencedParameter<object[]>("parameters");
          //           simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
          //        })
          //        .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'parameters' in the method Before of the type 'A.MyAspectAttribute'")))
          //        .AndLaunchTest();
       }

        [Test]
        public void CheckParametersBadType()
       {
           throw new NotImplementedException();
           //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
           //         .ByDefiningAssembly(simpleClassAndWeaver =>
           //        {
           //           simpleClassAndWeaver.BeforeInterceptor.WithParameter<int>("parameters");
           //           simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
           //        })
           //         .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the parameters parameter in the method Before of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object[]")))
           //         .AndLaunchTest();
        }

       [Test]
       public void CheckParametersWithRealType()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
          //         .ByDefiningAssembly(simpleClassAndWeaver =>
          //         {
          //            simpleClassAndWeaver.BeforeInterceptor.WithParameter<object[]>("parameters");
          //            simpleClassAndWeaver.MethodToWeave.WithParameter<int>("first");
          //         })
          //         .AndEnsureAssembly((assemblyP, result) =>
          //         {
          //            var o = result.CreateObjectFromClassToWeaveType();
          //            result.CallWeavedMethod(o, 12);
          //            Assert.AreEqual(new object[]
          //               {
          //                  12
          //               }, result.Aspect.BeforeParameters);

          //         })
          //         .AndLaunchTest();
       }
   }
}