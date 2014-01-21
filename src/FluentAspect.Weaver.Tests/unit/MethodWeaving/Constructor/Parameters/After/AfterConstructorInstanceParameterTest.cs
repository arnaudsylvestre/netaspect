using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After
{
    [TestFixture]
   public class AfterConstructorInstanceParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
        {
            throw new NotImplementedException();
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                  {
                     simpleClassAndWeaver.AfterInterceptor.WithReferencedParameter<object>("instance");
                  })
                  .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'instance' in the method After of the type 'A.MyAspectAttribute'")))
                  .AndLaunchTest();
       }

        [Test]
        public void CheckInstanceBadType()
       {
           throw new NotImplementedException();
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.AfterInterceptor.WithParameter<int>("instance");
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
                    .AndLaunchTest();
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            throw new NotImplementedException();
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.AfterInterceptor.WithParameter<object>("instance");
                   })
                   .AndEnsureAssembly((assemblyP, result) =>
                   {
                      var o = result.CreateObjectFromClassToWeaveType();
                      result.CallWeavedMethod(o);
                      Assert.AreEqual(o, result.Aspect.AfterInstance);

                   })
                   .AndLaunchTest();
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           throw new NotImplementedException();
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                    {                       
                        simpleClassAndWeaver.AfterInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
                    })
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       result.CallWeavedMethod(o);
                       Assert.AreEqual(o, result.Aspect.AfterInstance);

                    })
                    .AndLaunchTest();
        }
   }
}