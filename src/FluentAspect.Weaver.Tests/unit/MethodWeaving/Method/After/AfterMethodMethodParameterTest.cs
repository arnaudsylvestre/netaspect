﻿using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.After
{
    [TestFixture]
    public class AfterMethodMethodParameterTest
    {
       [Test]
       public void CheckMethodReferenced()
        {
            throw new NotImplementedException();
          DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                  {
                     simpleClassAndWeaver.AfterInterceptor.WithReferencedParameter<MethodInfo>("method");
                  })
                  .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'method' in the method After of the type 'A.MyAspectAttribute'")))
                  .AndLaunchTest();
       }

        [Test]
        public void CheckMethodBadType()
       {
           throw new NotImplementedException();
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                      simpleClassAndWeaver.AfterInterceptor.WithParameter<int>("method");
                   })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo")))
                    .AndLaunchTest();
        }

       [Test]
        public void CheckMethodWithRealType()
        {
            throw new NotImplementedException();
           DoUnit.Test(new SimpleClassAndWeaverAcceptanceTestBuilder())
                    .ByDefiningAssembly(simpleClassAndWeaver =>
                    {                       
                        simpleClassAndWeaver.AfterInterceptor.WithParameter<MethodInfo>("method");
                    })
                    .AndEnsureAssembly((assemblyP, result) =>
                    {
                       var o = result.CreateObjectFromClassToWeaveType();
                       result.CallWeavedMethod(o);
                       Assert.AreEqual("MyMethodToWeave", ((MethodInfo)result.Aspect.AfterMethod).Name);

                    })
                    .AndLaunchTest();
        }
   }
}