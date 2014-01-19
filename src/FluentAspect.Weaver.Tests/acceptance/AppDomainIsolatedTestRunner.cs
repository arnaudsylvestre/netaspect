﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Errors;
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

        public string Run(string dll_L, List<string> checkErrors, List<string> failures, List<string> warnings)
        {
            PEVerify verify_L = new PEVerify();
            verify_L.Run(dll_L);
            WeaverCore weaver = WeaverCoreFactory.Create();
            ErrorHandler errorHandler = new ErrorHandler();
            weaver.Weave(dll_L, errorHandler, (a) => a);
            var builder = new StringBuilder();
            ErrorsTest.Dump(errorHandler, builder);
            Assert.AreEqual(warnings, errorHandler.Warnings);
            Assert.AreEqual(checkErrors, errorHandler.Errors);
            Assert.AreEqual(failures, errorHandler.Failures);
            return builder.ToString();
        }

        public void Ensure<TSample, TActual>(string dll_L, IAcceptanceTestBuilder<TSample, TActual> context, Action<Assembly, TActual> ensure)
        {
            var assemblyDll = Assembly.LoadFrom(dll_L);
            ensure(assemblyDll, context.CreateActual(assemblyDll));
        }
    }
}