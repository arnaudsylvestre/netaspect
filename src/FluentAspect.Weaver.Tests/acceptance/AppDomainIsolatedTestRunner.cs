using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Errors;
using NUnit.Framework;
using System.Linq;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public void Run(Action action)
        {
            action();
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

        public string RunFromType(string dll_L, string typeName, List<string> checkErrors, List<string> failures, List<string> warnings)
        {
            var assembly = Assembly.LoadFrom(dll_L);
            var type = assembly.GetTypes().First(t => t.FullName == typeName);
            PEVerify verify_L = new PEVerify();
            verify_L.Run(dll_L);
            WeaverCore weaver = WeaverCoreFactory.Create();
            ErrorHandler errorHandler = new ErrorHandler();
            weaver.Weave(new[]{type}, errorHandler, (a) => a);
            var builder = new StringBuilder();
            ErrorsTest.Dump(errorHandler, builder);
            Assert.AreEqual(warnings, errorHandler.Warnings);
            Assert.AreEqual(checkErrors, errorHandler.Errors);
            Assert.AreEqual(failures, errorHandler.Failures);
            return builder.ToString();
        }

        public void Ensure(string dll_L,  Action<Assembly> ensure)
        {
           ensure(Assembly.LoadFrom(dll_L));
        }
    }
}