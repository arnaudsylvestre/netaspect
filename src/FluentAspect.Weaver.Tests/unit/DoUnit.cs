using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Tests.acceptance;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.unit
{
    public static class DoUnit
    {


           private static string AssemblyDirectory
            {
                get
                {
                    string codeBase = typeof(DoUnit).Assembly.CodeBase;
                    var uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    return Path.GetDirectoryName(path);
                }
            }



           private static AppDomainIsolatedTestRunner CreateAppRunner()
            {
                return AppDomain.CreateDomain("For test", null,
                                              new AppDomainSetup
                                                  {
                                                      ApplicationBase = AssemblyDirectory,
                                                      ShadowCopyFiles = "true"
                                                  }).CreateInstanceFromAndUnwrap(new Uri(typeof(AppDomainIsolatedTestRunner).Assembly.CodeBase).LocalPath,
                                                                                 typeof(AppDomainIsolatedTestRunner).FullName)
                       as AppDomainIsolatedTestRunner;
            }

            public static void Run<T>(Action<ErrorHandler> errorHandlerProvider, Action ensureAssembly)
           {
               var runner = CreateAppRunner();

               var errorHandler = new ErrorHandler();
               errorHandlerProvider(errorHandler);
                var dll = typeof (DoUnit).Assembly.GetAssemblyPath();
                Console.Write(runner.RunFromType(dll, typeof(T).FullName, errorHandler.Errors, errorHandler.Failures, errorHandler.Warnings));

               runner = CreateAppRunner();
               runner.Ensure(ensureAssembly);
           }
    }
}