using System;
using System.IO;
using System.Reflection;
using FluentAspect.Sample;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public void Run(Action action)
        {
            action();
        }
    }

    [TestFixture]
    public class WeaverToolTest
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = typeof (WeaverToolTest).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private AppDomainIsolatedTestRunner runner;

        [TestFixtureSetUp]
        public void DoWeaving()
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
                domain.CreateInstanceFromAndUnwrap(new Uri(runnerType.Assembly.CodeBase).LocalPath, runnerType.FullName)
                as AppDomainIsolatedTestRunner;
        }

        private static void Weave()
        {
            try
            {
                const string asm = "FluentAspect.Sample.exe";
                WeaverCore weaver = WeaverCoreFactory.Create();
                var errorHandler = new ErrorHandler();
                weaver.Weave(asm, asm, errorHandler);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        [Test]
        public void CheckBefore()
        {
            runner.Run(() =>
            {
                string res = new MyClassToWeave().CheckBefore(new BeforeParameter { Value = "not before" });
                Assert.AreEqual("Value set in before", res);
            });
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void CheckConstructor()
        {
            runner.Run(() =>
            {
                new MyClassToWeaveWithAttributes(true);
            });
        }

        [Test]
        public void CheckBeforeWithAttributes()
        {
            runner.Run(() =>
                {
                    string res =
                        new MyClassToWeaveWithAttributes().CheckBeforeWithAttributes(new BeforeParameter
                            {
                                Value = "not before"
                            });
                    Assert.AreEqual("Value set in before", res);
                });
        }

        [Test]
        public void CheckMock()
        {
            runner.Run(() =>
                {
                    MockInterceptor.after = null;
                    MockInterceptor.before = null;
                    MockInterceptor.exception = null;
                    var myClassToWeave = new MyClassToWeave();
                    string res = myClassToWeave.CheckMock("param");
                    Assert.AreSame(myClassToWeave, MockInterceptor.before.@this);
                    Assert.AreEqual("CheckMock", MockInterceptor.before.methodInfo_P.Name);
                    Assert.AreEqual(new object[] {"param"}, MockInterceptor.before.parameters);
                    Assert.AreSame(myClassToWeave, MockInterceptor.after.@this);
                    Assert.AreEqual("CheckMock", MockInterceptor.after.methodInfo_P.Name);
                    Assert.AreEqual(new object[] {"param"}, MockInterceptor.after.parameters);
                    Assert.AreEqual(res, MockInterceptor.after.result);
                    Assert.AreEqual(null, MockInterceptor.exception);
                });
        }

        [Test]
        public void CheckMockException()
        {
            runner.Run(() =>
                {
                    MockInterceptor.after = null;
                    MockInterceptor.before = null;
                    MockInterceptor.exception = null;
                    var myClassToWeave = new MyClassToWeave();
                    try
                    {
                        string res = myClassToWeave.CheckMockException();
                    }
                    catch (NotImplementedException)
                    {
                        Assert.AreSame(myClassToWeave, MockInterceptor.before.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptor.before.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptor.before.parameters);
                        Assert.AreSame(myClassToWeave, MockInterceptor.exception.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptor.exception.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptor.exception.parameters);
                        Assert.True(MockInterceptor.exception.Exception is NotImplementedException);
                    }
                });
        }

        [Test]
        public void CheckNotRenameInAssembly()
        {
            runner.Run(() =>
                {
                    string res = new MyClassToWeave().CheckNotRenameInAssembly();
                    Assert.AreEqual("Weaved", res);
                });
        }

        [Test]
        public void CheckStatic()
        {
            runner.Run(() =>
                {
                    string res = MyClassToWeave.CheckStatic(new BeforeParameter {Value = "not before"});
                    Assert.AreEqual("Value set in before", res);
                });
        }

        [Test, ExpectedException(typeof (NotSupportedException))]
        public void CheckThrow()
        {
            runner.Run(() =>
                {
                    try
                    {
                        new MyClassToWeave().CheckThrow();
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                });
        }

        [Test]
        public void CheckWithGenerics()
        {
            runner.Run(() =>
                {
                    string res = new MyClassToWeave().CheckWithGenerics("Weaved");
                    Assert.AreEqual("Weaved<>System.StringWeaved", res);
                });
        }

        [Test]
        public void CheckWithParameters()
        {
            runner.Run(() =>
                {
                    string res = new MyClassToWeave().CheckWithParameters("Weaved with parameters");
                    Assert.AreEqual("Weaved with parameters", res);
                });
        }

        [Test]
        public void CheckWithReturn()
        {
            runner.Run(() =>
                {
                    string res = new MyClassToWeave().CheckWithReturn();
                    Assert.AreEqual("Weaved", res);
                });
        }

        [Test]
        public void CheckWithVoid()
        {
            runner.Run(() => { new MyClassToWeave().CheckWithVoid(); });
        }
    }
}