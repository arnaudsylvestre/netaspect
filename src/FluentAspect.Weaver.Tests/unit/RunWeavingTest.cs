using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Tests.Helpers;

namespace FluentAspect.Weaver.Tests.unit
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

        public static void For<T>(Action<ErrorHandler> errorHandlerProvider, Action ensureAssembly)
        {
            var assembly = typeof(RunWeavingTest).Assembly;
            AppDomainIsolatedTestRunner runner = CreateAppRunner(assembly);

            var errorHandler = new ErrorHandler();
            errorHandlerProvider(errorHandler);
            string dll = assembly.GetAssemblyPath();
            Console.Write(runner.RunFromType(dll, typeof (T).FullName, errorHandler.Errors, errorHandler.Failures,
                                             errorHandler.Warnings));

            runner = CreateAppRunner(assembly);
            runner.Ensure(ensureAssembly);
        }
    }
}