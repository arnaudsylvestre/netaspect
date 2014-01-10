using System;
using System.Reflection;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit
{
    [TestFixture]
    public abstract class SimpleAcceptanceTest
    {
       protected abstract void ConfigureAssembly(AssemblyDefinitionDefiner assembly);

        [Test]
        public void Launch()
        {
            var assembly = AssemblyBuilder.Create();
           ConfigureAssembly(assembly);
            
            //method.WhichContainsThrowException();
           var dll_L = "TempAssembly.dll";
           assembly.Save(dll_L);
           PEVerify verify_L = new PEVerify();
           verify_L.Run(dll_L);
            WeaverCore weaver = WeaverCoreFactory.Create();
           ErrorHandler errorHandler = new ErrorHandler();
           weaver.Weave(dll_L, errorHandler, (a) => a);
           var assembly_L = Assembly.LoadFrom(dll_L);
           EnsureErrors(errorHandler);
           EnsureAssembly(assembly_L);
        }

       protected abstract void EnsureAssembly(Assembly assembly_P);

       protected virtual void EnsureErrors(ErrorHandler errorHandler)
       {
          ErrorsTest.Dump(errorHandler);
          Assert.AreEqual(0, errorHandler.Warnings.Count);
          Assert.AreEqual(0, errorHandler.Errors.Count);
          Assert.AreEqual(0, errorHandler.Failures.Count);
       }
    }
}