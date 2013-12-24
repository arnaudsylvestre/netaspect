using System;
using System.Collections.Generic;
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
            if (WeaverToolTest.errorHandler.Errors.Count != 0 || WeaverToolTest.errorHandler.Warnings.Count != 0)
                throw new Exception("Errors or warnings");
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

        public static ErrorHandler errorHandler = new ErrorHandler();

        private static void Weave()
        {
            try
            {
                const string asm = "FluentAspect.Sample.exe";
                WeaverCore weaver = WeaverCoreFactory.Create();
                weaver.Weave(asm, errorHandler, (a) => asm);

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
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CheckDependency()
        {
            runner.Run(() =>
            {
                new MyClassToWeave().CheckDependency(null);
            });
        }
        [Test]
        public void CheckDependencyWithoutException()
        {
            runner.Run(() =>
            {
                new MyClassToWeave().CheckDependency("");
            });
        }

        [Test]
        public void CheckByParameterName()
        {
            runner.Run(() =>
            {
                string res = new MyClassToWeave().CheckWithParameterName(6, 1);
                Assert.AreEqual("6 : 7", res);
            });
        }

        [Test]
        public void CheckMultiInterceptors()
        {
            runner.Run(() =>
            {
                var res = new MyClassToWeave().CheckMulti(1);
                Assert.AreEqual("3", res);
            });
        }

        [Test]
        public void CheckErrors()
        {
            Assert.AreEqual(new List<string>()
            {
                "A method declared in interface can not be weaved : InterfaceToWeaveWithAttributes.CheckBeforeWithAttributes",       
                "An abstract method can not be weaved : AbstractClassToWeaveWithAttributes.CheckBeforeWithAttributes",
            }, errorHandler.Warnings);
            Assert.AreEqual(new List<string>()
            {

            }, errorHandler.Errors);
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
        public void CheckConstructorWithFalse()
        {
            runner.Run(() =>
            {
                new MyClassToWeaveWithAttributes(false);
            });
        }

        [Test]
        public void CheckPropertyGetter()
        {
            runner.Run(() =>
            {
                Assert.AreEqual("3", new MyClassToWeaveWithAttributes(false).PropertyGetter);
            });
        }

        [Test]
        public void CheckBeforeWithAttributes()
        {
            runner.Run(() =>
            {
                string res =
                    new MyClassToWeaveWithAttributes(false).CheckBeforeWithAttributes(new BeforeParameter
                    {
                        Value = "not before"
                    });
                Assert.AreEqual("Value set in before", res);
            });
        }

        [Test]
        public void CheckInterceptPrivate()
        {
            runner.Run(() =>
            {
                string res =
                    new MyClassToWeaveWithAttributes(false).CallCheckBeforeWithAttributesPrivate(new BeforeParameter
                    {
                        Value = "not before"
                    });
                Assert.AreEqual("Value set in before", res);
            });
        }

        [Test]
        public void CheckInterceptProtected()
        {
            runner.Run(() =>
            {
                string res =
                    new MyClassToWeaveWithAttributes(false).CallCheckBeforeWithAttributesProtected(new BeforeParameter
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
                    MockInterceptorNetAspectAttribute.after = null;
                    MockInterceptorNetAspectAttribute.before = null;
                    MockInterceptorNetAspectAttribute.exception = null;
                    var myClassToWeave = new MyClassToWeave();
                    string res = myClassToWeave.CheckMock("param");
                    Assert.AreSame(myClassToWeave, MockInterceptorNetAspectAttribute.before.@this);
                    Assert.AreEqual("CheckMock", MockInterceptorNetAspectAttribute.before.methodInfo_P.Name);
                    Assert.AreEqual(new object[] { "param" }, MockInterceptorNetAspectAttribute.before.parameters);
                    Assert.AreSame(myClassToWeave, MockInterceptorNetAspectAttribute.after.@this);
                    Assert.AreEqual("CheckMock", MockInterceptorNetAspectAttribute.after.methodInfo_P.Name);
                    Assert.AreEqual(new object[] { "param" }, MockInterceptorNetAspectAttribute.after.parameters);
                    Assert.AreEqual(res, MockInterceptorNetAspectAttribute.after.result);
                    Assert.AreEqual(null, MockInterceptorNetAspectAttribute.exception);
                });
        }

        [Test]
        public void CheckMockException()
        {
            runner.Run(() =>
                {
                    MockInterceptorNetAspectAttribute.after = null;
                    MockInterceptorNetAspectAttribute.before = null;
                    MockInterceptorNetAspectAttribute.exception = null;
                    var myClassToWeave = new MyClassToWeave();
                    try
                    {
                        string res = myClassToWeave.CheckMockException();
                    }
                    catch (NotImplementedException)
                    {
                        Assert.AreSame(myClassToWeave, MockInterceptorNetAspectAttribute.before.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptorNetAspectAttribute.before.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptorNetAspectAttribute.before.parameters);
                        Assert.AreSame(myClassToWeave, MockInterceptorNetAspectAttribute.exception.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptorNetAspectAttribute.exception.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptorNetAspectAttribute.exception.parameters);
                        Assert.True(MockInterceptorNetAspectAttribute.exception.Exception is NotImplementedException);
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




        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Weaved through assembly")]
        public void CheckWeavedThroughAssembly()
        {
            runner.Run(() =>
            {
                try
                {
                    new MyClassToWeave().WeavedThroughAssembly();
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
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

        [Test, ExpectedException(typeof(NotSupportedException))]
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

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void CheckThrowWithReturn()
        {
            runner.Run(() =>
            {
                try
                {
                    new MyClassToWeave().CheckThrowWithReturn();
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
        public void CheckWithReturnSimpleType()
        {
            runner.Run(() =>
            {
                int res = new MyClassToWeave().CheckWithReturnSimpleType();
                Assert.AreEqual(5, res);
            });
        }

        [Test]
        public void CheckWithVoid()
        {
            runner.Run(() => { new MyClassToWeave().CheckWithVoid(); });
        }
    }
}