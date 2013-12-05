using System;
using System.Reflection;
using FluentAspect.Sample;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{

    public class AppDomainIsolatedDiscoveryRunner : MarshalByRefObject
    {
        public void Process(Action action)
        {
            action();
        }
    }

    [TestFixture]
    [Serializable]
    public class WeaverToolTest
    {
        [Test]
        public void CheckBefore()
        {
            string res = new MyClassToWeave().CheckBefore(new BeforeParameter { Value = "not before" });
            Assert.AreEqual("Value set in before", res);
        }

        [Test]
        public void CheckBefore2()
        {
            Weave();
            var domain = AppDomain.CreateDomain("For test", null,
                        new AppDomainSetup
                        {
                            ApplicationBase = @"D:\Developpement\fluentaspect\src\FluentAspect.Weaver.Tests\bin\Debug",                            
                            ShadowCopyFiles = "true"
                        });
            var runnerType = typeof(AppDomainIsolatedDiscoveryRunner);

            var runner = domain.CreateInstanceFromAndUnwrap(new Uri(runnerType.Assembly.CodeBase).LocalPath, runnerType.FullName) as AppDomainIsolatedDiscoveryRunner;
            runner.Process(CheckBefore);
        }

        [Test]
        public void CheckNotRenameInAssembly()
        {
            string res = new MyClassToWeave().CheckNotRenameInAssembly();
            Assert.AreEqual("Weaved", res);
        }

        [Test]
        public void CheckStatic()
        {
            string res = MyClassToWeave.CheckStatic(new BeforeParameter {Value = "not before"});
            Assert.AreEqual("Value set in before", res);
        }

        [Test, ExpectedException(typeof (NotSupportedException))]
        public void CheckThrow()
        {
            try
            {
                new MyClassToWeave().CheckThrow();
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [Test]
        public void CheckBeforeWithAttributes()
        {
            string res = new MyClassToWeaveWithAttributes().CheckBeforeWithAttributes(new BeforeParameter {Value = "not before"});
            Assert.AreEqual("Value set in before", res);
        }

        [Test]
        public void CheckWithGenerics()
        {
            string res = new MyClassToWeave().CheckWithGenerics("Weaved");
            Assert.AreEqual("Weaved<>System.StringWeaved", res);
        }

        [Test]
        public void CheckWithParameters()
        {
            string res = new MyClassToWeave().CheckWithParameters("Weaved with parameters");
            Assert.AreEqual("Weaved with parameters", res);
        }

        [Test]
        public void CheckWithReturn()
        {
            string res = new MyClassToWeave().CheckWithReturn();
            Assert.AreEqual("Weaved", res);
        }

        [Test]
        public void CheckMock()
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
            Assert.AreEqual(new object[] { "param" }, MockInterceptor.after.parameters);
            Assert.AreEqual(res, MockInterceptor.after.result);
            Assert.AreEqual(null, MockInterceptor.exception);
        }

        [Test]
        public void CheckMockException()
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
        }

        [Test]
        public void CheckWithVoid()
        {
            new MyClassToWeave().CheckWithVoid();
        }

        [Test, Ignore]
        public void Weave()
        {
            const string asm = "FluentAspect.Sample.exe";
            WeaverCore weaver = WeaverCoreFactory.Create();
            weaver.Weave(asm, asm);
        }
    }
}