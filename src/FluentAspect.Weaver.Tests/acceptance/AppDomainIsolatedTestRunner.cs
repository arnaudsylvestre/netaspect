using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Errors;
using FluentAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public void Run(Action action)
        {
            action();
        }



        public void Run(string dll_L, Action<ErrorHandler> checkErrors)
        {

            PEVerify verify_L = new PEVerify();
            verify_L.Run(dll_L);
            WeaverCore weaver = WeaverCoreFactory.Create();
            ErrorHandler errorHandler = new ErrorHandler();
            weaver.Weave(dll_L, errorHandler, (a) => a);
            checkErrors(errorHandler);
        }

        public void Run(string dll_L, List<string> checkErrors, List<string> failures, List<string> warnings)
        {
            PEVerify verify_L = new PEVerify();
            verify_L.Run(dll_L);
            WeaverCore weaver = WeaverCoreFactory.Create();
            ErrorHandler errorHandler = new ErrorHandler();
            weaver.Weave(dll_L, errorHandler, (a) => a);
            ErrorsTest.Dump(errorHandler);
            Assert.AreEqual(warnings, errorHandler.Warnings);
            Assert.AreEqual(checkErrors, errorHandler.Errors);
            Assert.AreEqual(failures, errorHandler.Failures);
        }

        public void Ensure(string dll_L, Action<Assembly, DoAcceptanceHelper> ensure)
        {
            var assemblyDll = Assembly.LoadFrom(dll_L);
            ensure(assemblyDll, new DoAcceptanceHelper(assemblyDll));
        }
    }
}