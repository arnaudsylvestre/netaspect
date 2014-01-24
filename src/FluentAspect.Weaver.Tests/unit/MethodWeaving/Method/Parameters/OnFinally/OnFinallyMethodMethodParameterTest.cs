using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally
{
    [TestFixture]
    public class OnFinallyMethodMethodParameterTest
    {
       [Test]
       public void CheckMethodReferenced()
       {

          throw new NotImplementedException();
          //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //        {
          //           simpleClassAndWeaver.OnFinallyInterceptor.WithReferencedParameter<MethodInfo>("method");
          //        })
          //        .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'method' in the method OnFinally of the type 'A.MyAspectAttribute'")))
          //        .AndEnsureAssembly((assemblyP, result) =>
          //        {
          //           var o = result.CreateObjectFromClassToWeaveType();
          //           try
          //           {

          //              result.CallWeavedMethod(o);
          //              Assert.Fail();
          //           }
          //           catch (Exception)
          //           {
          //              Assert.AreEqual(null, result.Aspect.OnFinallyMethod);
          //           }

          //        })
          //        .AndLaunchTest();
       }

        [Test]
        public void CheckMethodBadType()
        {

           throw new NotImplementedException();
           //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
           //         .ByDefiningAssembly(simpleClassAndWeaver =>
           //        {
           //           simpleClassAndWeaver.OnFinallyInterceptor.WithParameter<int>("method");
           //           simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
           //        })
           //         .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method OnFinally of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo")))
           //         .AndEnsureAssembly((assemblyP, result) =>
           //         {
           //            var o = result.CreateObjectFromClassToWeaveType();
           //            try
           //            {

           //               result.CallWeavedMethod(o);
           //               Assert.Fail();
           //            }
           //            catch (Exception)
           //            {
           //               Assert.AreEqual(0, result.Aspect.OnFinallyMethod);
           //            }

           //         })
           //         .AndLaunchTest();
        }


       [Test]
        public void CheckMethodWithRealType()
        {

           throw new NotImplementedException();
           //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
           //         .ByDefiningAssembly(simpleClassAndWeaver =>
           //         {                       
           //             simpleClassAndWeaver.OnFinallyInterceptor.WithParameter<MethodInfo>("method");
           //            simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
           //         })
           //         .AndEnsureAssembly((assemblyP, result) =>
           //         {
           //            var o = result.CreateObjectFromClassToWeaveType();
           //            try
           //            {

           //               result.CallWeavedMethod(o);
           //               Assert.Fail();
           //            }
           //            catch (Exception)
           //            {
           //               Assert.AreEqual("MyMethodToWeave", ((MethodInfo)result.Aspect.OnFinallyMethod).Name);
           //            }

           //         })
           //         .AndLaunchTest();
        }
   }
}