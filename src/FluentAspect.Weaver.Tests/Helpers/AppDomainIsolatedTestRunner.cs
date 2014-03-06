using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Helpers
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public string RunFromType(string dll_L, string typeName, List<string> checkErrors, List<string> failures,
                                  List<string> warnings)
        {
            Assembly assembly = Assembly.LoadFrom(dll_L);
            Type type = assembly.GetTypes().First(t => t.FullName == typeName);
            WeaverCore weaver = WeaverCoreFactory.Create();
            var errorHandler = new ErrorHandler();
            weaver.Weave(ComputeTypes(type), errorHandler, (a) => a);
            var builder = new StringBuilder();
            errorHandler.Dump(builder);
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