using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    [TestFixture]
    public class OnExceptionConstructorParametersParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
       {
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                  {
                     simpleClassAndWeaver.OnExceptionInterceptor.WithReferencedParameter<object>("instance");
                  })
                  .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'instance' in the method OnException of the type 'A.MyAspectAttribute'")))
                  .AndEnsureAssembly((assemblyP, result) =>
                  {
                     var o = result.CreateObjectFromClassToWeaveType();
                     try
                     {

                        result.CallWeavedMethod(o);
                        Assert.Fail();
                     }
                     catch (Exception)
                     {
                        Assert.AreEqual(null, result.Aspect.OnExceptionInstance);
                     }

                  })
                  .AndLaunchTest();
       }

        [Test]
        public void CheckInstanceBadType()
        {
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.OnExceptionInterceptor.WithParameter<int>("instance");
                      simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method OnException of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       try
                       {

                          result.CallWeavedMethod(o);
                          Assert.Fail();
                       }
                       catch (Exception)
                       {
                          Assert.AreEqual(0, result.Aspect.OnExceptionInstance);
                       }

                    })
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
       {
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.OnExceptionInterceptor.WithParameter<object>("instance");
                      simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
                   })
                   .AndEnsureAssembly((assemblyP, result) =>
                   {
                      var o = result.CreateObjectFromClassToWeaveType();
                      try
                      {
                         result.CallWeavedMethod(o);
                         Assert.Fail();
                      }
                      catch (Exception)
                      {
                         Assert.AreEqual(o, result.Aspect.OnExceptionInstance);
                      }

                   })
                   .AndLaunchTest();
       }


       [Test]
        public void CheckInstanceWithRealType()
        {
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                    {                       
                        simpleClassAndWeaver.OnExceptionInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
                       simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
                    })
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       try
                       {

                          result.CallWeavedMethod(o);
                          Assert.Fail();
                       }
                       catch (Exception)
                       {
                          Assert.AreEqual(o, result.Aspect.OnExceptionInstance);
                       }

                    })
                    .AndLaunchTest();
        }
   }
}