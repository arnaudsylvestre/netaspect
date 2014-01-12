using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public static class DoAcceptance
    {
        public static DoAcceptanceConfiguration<T> Test<T>(T context)
        {
            return new DoAcceptanceConfiguration<T>(context);
        }
        public static DoAcceptanceConfiguration<object> Test()
        {
            return new DoAcceptanceConfiguration<object>(null);
        }

        public class DoAcceptanceConfiguration<T>
        {
            private readonly T _context;
            private Action<AssemblyDefinitionDefiner> configure = definer => { };
            private Action<Assembly, T, DoAcceptanceHelper> ensure = (assembly, t, helper) => { };
            private Action<ErrorHandler> errorHandlerProvider = e => { };

            public DoAcceptanceConfiguration(T context)
            {
                _context = context;
            }


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

            public DoAcceptanceConfiguration<T> ByDefiningAssembly(Action<AssemblyDefinitionDefiner> configure)
            {
                this.configure = configure;
                return this;
            }

            public DoAcceptanceConfiguration<T> AndEnsureAssembly(Action<Assembly, T, DoAcceptanceHelper> ensure)
            {
                this.ensure = ensure;
                return this;
            }

            public DoAcceptanceConfiguration<T> EnsureErrorHandler(Action<ErrorHandler> errorHandlerProvider)
            {
                this.errorHandlerProvider = errorHandlerProvider;
                return this;
            }

            private const string dll_L = "Temp.dll";

            public void AndLaunchTest()
            {
                var runner = CreateAppRunner();

                var assembly = AssemblyBuilder.Create();
                configure(assembly);
                assembly.Save(dll_L);

                var errorHandler = new ErrorHandler();
                errorHandlerProvider(errorHandler);
                Console.Write(runner.Run(dll_L, errorHandler.Errors, errorHandler.Failures, errorHandler.Warnings));

                runner = CreateAppRunner();
                runner.Ensure(dll_L, _context, ensure);
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
        }

    }
}