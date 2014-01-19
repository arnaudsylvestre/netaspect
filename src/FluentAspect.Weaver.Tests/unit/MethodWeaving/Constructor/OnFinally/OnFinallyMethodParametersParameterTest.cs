using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.OnFinally
{
    [TestFixture]
    public class OnFinallyMethodParametersParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
       {
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                  {
                     simpleClassAndWeaver.OnFinallyInterceptor.WithReferencedParameter<object>("instance");
                  })
                  .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'instance' in the method OnFinally of the type 'A.MyAspectAttribute'")))
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
                        Assert.AreEqual(null, result.Aspect.OnFinallyInstance);
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
                      simpleClassAndWeaver.OnFinallyInterceptor.WithParameter<int>("instance");
                      simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method OnFinally of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
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
                          Assert.AreEqual(0, result.Aspect.OnFinallyInstance);
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
                      simpleClassAndWeaver.OnFinallyInterceptor.WithParameter<object>("instance");
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
                         Assert.AreEqual(o, result.Aspect.OnFinallyInstance);
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
                        simpleClassAndWeaver.OnFinallyInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
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
                          Assert.AreEqual(o, result.Aspect.OnFinallyInstance);
                       }

                    })
                    .AndLaunchTest();
        }
   }
}