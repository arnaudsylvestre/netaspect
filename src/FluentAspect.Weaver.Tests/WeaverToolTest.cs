using System;
using System.Reflection;
using FluentAspect.Sample;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Factory;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverToolTest
    {
        [Test]
        public void CheckBefore()
        {
            string res = new MyClassToWeave().CheckBefore(new BeforeParameter {Value = "not before"});
            Assert.AreEqual("Value set in before", res);
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
            string res = myClassToWeave.CheckMock();
            Assert.AreSame(myClassToWeave, MockInterceptor.before.@this);
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