﻿using System;
using System.IO;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using FluentAspect.Weaver.Tests.acceptance;

namespace FluentAspect.Weaver.Tests.unit
{
    public static class DoUnit2
    {
       public static DoAcceptanceConfiguration Test()
        {
           return new DoAcceptanceConfiguration();
        }

       public class DoAcceptanceConfiguration
        {
          private Action<NetAspectAssembly> configure = definer => { };
          private Action<Assembly> ensure = (assembly) => { };
            private Action<ErrorHandler> errorHandlerProvider = e => { };

            public DoAcceptanceConfiguration()
            {
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

          public DoAcceptanceConfiguration ByDefiningAssembly(Action<TSample> configure)
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