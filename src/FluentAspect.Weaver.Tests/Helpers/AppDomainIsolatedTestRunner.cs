using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;
using System.Linq;

namespace FluentAspect.Weaver.Tests.Helpers
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public string RunFromType(string dll_L, string typeName, List<string> checkErrors, List<string> failures, List<string> warnings)
        {
            var assembly = Assembly.LoadFrom(dll_L);
            var type = assembly.GetTypes().First(t => t.FullName == typeName);
            var weaver = WeaverCoreFactory.CreateV2();
            var errorHandler = new ErrorHandler();
            weaver.Weave(ComputeTypes(type), errorHandler, (a) => a);
            var builder = new StringBuilder();
            ErrorHandlerExtensions.Dump(errorHandler, builder);
            Assert.AreEqual(warnings, errorHandler.Warnings);
            Assert.AreEqual(checkErrors, errorHandler.Errors);
            Assert.AreEqual(failures, errorHandler.Failures);
            return builder.ToString();
        }

        private static Type[] ComputeTypes(Type type)
        {
            return type.DeclaringType.GetNestedTypes();
        }

        public void Ensure(Action ensure)
        {
           ensure();
        }
    }
}