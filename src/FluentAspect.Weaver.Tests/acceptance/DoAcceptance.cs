using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public static class DoAcceptance
    {
       public static DoAcceptanceConfiguration<TSample, TActual> Test<TSample, TActual>(IAcceptanceTestBuilder<TSample, TActual> acceptanceTestBuilder)
        {
           return new DoAcceptanceConfiguration<TSample, TActual>(acceptanceTestBuilder);
        }

       public class DoAcceptanceConfiguration<TSample, TActual>
        {
          private readonly IAcceptanceTestBuilder<TSample, TActual> _acceptanceTestBuilder;
          private Action<TSample> configure = definer => { };
          private Action<Assembly, TActual> ensure = (assembly, t) => { };
            private Action<ErrorHandler> errorHandlerProvider = e => { };

            public DoAcceptanceConfiguration(IAcceptanceTestBuilder<TSample, TActual> acceptanceTestBuilder_P)
            {
               _acceptanceTestBuilder = acceptanceTestBuilder_P;
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

          public DoAcceptanceConfiguration<TSample, TActual> ByDefiningAssembly(Action<TSample> configure)
            {
                this.configure = configure;
                return this;
            }

          public DoAcceptanceConfiguration<TSample, TActual> AndEnsureAssembly(Action<Assembly, TActual> ensure)
            {
                this.ensure = ensure;
                return this;
            }

          public DoAcceptanceConfiguration<TSample, TActual> EnsureErrorHandler(Action<ErrorHandler> errorHandlerProvider)
            {
                this.errorHandlerProvider = errorHandlerProvider;
                return this;
            }

            private const string dll_L = "Temp.dll";

            public void AndLaunchTest()
            {
                var runner = CreateAppRunner();

                var assembly = AssemblyBuilder.Create();
               var sample_L = _acceptanceTestBuilder.CreateSample(assembly);
               configure(sample_L);
                assembly.Save(dll_L);

                var errorHandler = new ErrorHandler();
                errorHandlerProvider(errorHandler);
                Console.Write(runner.Run(dll_L, errorHandler.Errors, errorHandler.Failures, errorHandler.Warnings));

                runner = CreateAppRunner();
                runner.Ensure(dll_L, _acceptanceTestBuilder, ensure);
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