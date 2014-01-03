using System;
using System.IO;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance
{
    [TestFixture]
    public abstract class AcceptanceTest : MarshalByRefObject
    {
        private static bool weaved;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = typeof (AcceptanceTest).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [TestFixtureSetUp]
        public void DoWeaving()
        {
            if (!weaved)
            {
                Weave();

                AppDomain domain = AppDomain.CreateDomain("For test", null,
                                                          new AppDomainSetup
                                                              {
                                                                  ApplicationBase = AssemblyDirectory,
                                                                  ShadowCopyFiles = "true"
                                                              });
                Type runnerType = typeof (AppDomainIsolatedTestRunner);

                runner =
                    domain.CreateInstanceFromAndUnwrap(new Uri(runnerType.Assembly.CodeBase).LocalPath,
                                                       runnerType.FullName)
                    as AppDomainIsolatedTestRunner;
                weaved = true;
            }
        }


        protected virtual void EnsureErrorHandler(ErrorHandler errorHandler)
        {
        }

        protected abstract Action Execute();

        private static readonly ErrorHandler errorHandler = new ErrorHandler();
        private static AppDomainIsolatedTestRunner runner;

        private static void Weave()
        {
            try
            {
                const string asm = "FluentAspect.Sample.exe";
                WeaverCore weaver = WeaverCoreFactory.Create();
                weaver.Weave(asm, errorHandler, (a) => a);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        [Test]
        public void DoTest()
        {
            EnsureErrorHandler(errorHandler);
            runner.Run(Execute());
        }
    }
}