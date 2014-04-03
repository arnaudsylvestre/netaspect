using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Tests.Helpers;

namespace NetAspect.Weaver.Tests.unit
{
    public static class RunWeavingTest
    {
        private static AppDomainIsolatedTestRunner CreateAppRunner(Assembly assembly)
        {
            return AppDomain.CreateDomain("For test", null,
                                          new AppDomainSetup
                                              {
                                                  ApplicationBase = Path.GetDirectoryName(assembly.GetAssemblyPath()),
                                                  ShadowCopyFiles = "true"
                                              })
                            .CreateInstanceFromAndUnwrap(
                                new Uri(typeof (AppDomainIsolatedTestRunner).Assembly.CodeBase).LocalPath,
                                typeof (AppDomainIsolatedTestRunner).FullName)
                   as AppDomainIsolatedTestRunner;
        }

        public static void For<T, U>(Action<ErrorHandler> errorHandlerProvider, Action ensureAssembly)
        {
            Assembly assembly = typeof(T).Assembly;
            Assembly otherAssembly = typeof(U).Assembly;
            AppDomainIsolatedTestRunner runner = CreateAppRunner(assembly);

            var errorHandler = new ErrorHandler();
            errorHandlerProvider(errorHandler);
            string dll = assembly.GetAssemblyPath();
            var otherDll = otherAssembly.GetAssemblyPath();
            Console.Write(runner.RunFromType(dll, typeof(T).FullName, errorHandler.Errors, errorHandler.Failures,
                                             errorHandler.Warnings, otherDll, typeof(U).FullName));

            runner = CreateAppRunner(assembly);
            runner.Ensure(ensureAssembly);
        }
    }
}