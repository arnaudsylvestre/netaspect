﻿using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    [TestFixture]
   public class BeforeParametersParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
       {
          DoAcceptance.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                  {
                     simpleClassAndWeaver.BeforeInterceptor.WithReferencedParameter<object>("instance");
                  })
                  .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'instance' in the method Before of the type 'A.MyAspectAttribute'")))
                  .AndLaunchTest();
       }

        [Test]
        public void CheckInstanceBadType()
        {
           DoAcceptance.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.BeforeInterceptor.WithParameter<int>("instance");
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
       {
          DoAcceptance.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.BeforeInterceptor.WithParameter<object>("instance");
                   })
                   .AndEnsureAssembly((assemblyP, result) =>
                   {
                      var o = result.CreateObjectFromClassToWeaveType();
                      result.CallWeavedMethod(o);
                      Assert.AreEqual(o, result.Aspect.BeforeInstance);

                   })
                   .AndLaunchTest();
       }


       [Test]
        public void CheckInstanceWithRealType()
        {
           DoAcceptance.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                    {                       
                        simpleClassAndWeaver.BeforeInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
                    })
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       result.CallWeavedMethod(o);
                       Assert.AreEqual(o, result.Aspect.BeforeInstance);

                    })
                    .AndLaunchTest();
        }
   }
}