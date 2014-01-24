using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionConstructorMethodParameterTest
    {
       [Test]
       public void CheckMethodReferenced()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //        {
          //           simpleClassAndWeaver.OnExceptionInterceptor.WithReferencedParameter<MethodInfo>("method");
          //        })
          //        .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'method' in the method OnException of the type 'A.MyAspectAttribute'")))
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
          //              Assert.AreEqual(null, result.Aspect.OnExceptionMethod);
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
           //           simpleClassAndWeaver.OnExceptionInterceptor.WithParameter<int>("method");
           //           simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
           //        })
           //         .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method OnException of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo")))
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
           //               Assert.AreEqual(0, result.Aspect.OnExceptionMethod);
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
           //             simpleClassAndWeaver.OnExceptionInterceptor.WithParameter<MethodInfo>("method");
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
           //               Assert.AreEqual("MyMethodToWeave", ((MethodInfo)result.Aspect.OnExceptionMethod).Name);
           //            }

           //         })
           //         .AndLaunchTest();
        }
   }
}