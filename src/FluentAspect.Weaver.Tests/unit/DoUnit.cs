using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using FluentAspect.Weaver.Tests.acceptance;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.unit
{
    public static class DoUnit
    {
       public static DoAcceptanceConfiguration Test()
        {
           return new DoAcceptanceConfiguration();
        }

       public class DoAcceptanceConfiguration
        {
           private Action<AssemblyDefinition> configure = definer => { };
          private Action<Assembly> ensure = (assembly) => { };
            private Action<ErrorHandler> errorHandlerProvider = e => { };


           private static string AssemblyDirectory
            {
                get
                {
                    string codeBase = typeof(AcceptanceTest).Assembly.CodeBase;
                    var uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    return Path.GetDirectoryName(path);
                }
            }

          public DoAcceptanceConfiguration ByDefiningAssembly(Action<AssemblyDefinition> configure)
            {
                this.configure = configure;
                return this;
            }

          public DoAcceptanceConfiguration AndEnsureAssembly(Action<Assembly> ensure)
            {
                this.ensure = ensure;
                return this;
            }

          public DoAcceptanceConfiguration EnsureErrorHandler(Action<ErrorHandler> errorHandlerProvider)
            {
                this.errorHandlerProvider = errorHandlerProvider;
                return this;
            }

            private const string dll_L = "Temp.dll";

            public void AndLaunchTest()
            {
                var runner = CreateAppRunner();

                var assemblyDefinition = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("Temp", new Version("1.0")), "Temp", ModuleKind.Dll);

                configure(assemblyDefinition);
               assemblyDefinition.Write(dll_L, new WriterParameters()
                   {
                       WriteSymbols = true
                   });

                var errorHandler = new ErrorHandler();
                errorHandlerProvider(errorHandler);
                Console.Write(runner.Run(dll_L, errorHandler.Errors, errorHandler.Failures, errorHandler.Warnings));

                runner = CreateAppRunner();
                runner.Ensure(dll_L, ensure);
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
                var dll = typeof (DoAcceptanceConfiguration).Assembly.GetAssemblyPath();
                Console.Write(runner.RunFromType(dll, typeof(T).FullName, errorHandler.Errors, errorHandler.Failures, errorHandler.Warnings));

               runner = CreateAppRunner();
               runner.Ensure(ensureAssembly);
           }
        }
    }
}