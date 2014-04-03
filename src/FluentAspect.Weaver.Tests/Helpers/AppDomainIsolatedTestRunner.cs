using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Factory;

namespace NetAspect.Weaver.Tests.Helpers
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public string RunFromType(string dll_L, string typeName, List<string> checkErrors, List<string> failures, List<string> warnings, string otherDll, string otherTypeName)
        {
            Assembly assembly = Assembly.LoadFrom(dll_L);
            Assembly otherAssembly = Assembly.LoadFrom(otherDll);
            Type type = assembly.GetTypes().First(t => t.FullName == typeName);
            Type otherType = otherAssembly.GetTypes().First(t => t.FullName == otherTypeName);
            WeaverCore weaver = WeaverCoreFactory.Create();
            var errorHandler = new ErrorHandler();
            weaver.Weave(ComputeTypes(type, otherType), ComputeTypes(type, otherType), errorHandler, (a) => a);
            var builder = new StringBuilder();
            errorHandler.Dump(builder);
            Assert.AreEqual(warnings, errorHandler.Warnings);
            Assert.AreEqual(checkErrors, errorHandler.Errors);
            Assert.AreEqual(failures, errorHandler.Failures);
            return builder.ToString();
        }

        private static Type[] ComputeTypes(Type type, Type otherType)
        {
            var computeTypes = type.DeclaringType.GetNestedTypes().ToList();
            computeTypes.Add(otherType);
            return computeTypes.ToArray();
        }

        public void Ensure(Action ensure)
        {
            ensure();
        }
    }
}