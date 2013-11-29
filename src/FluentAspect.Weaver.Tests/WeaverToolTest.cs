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
        [Test, Ignore]
        public void LaunchWeaving()
        {
            const string asm = "FluentAspect.Sample.exe";
           var weaver_L = WeaverCoreFactory.Create();
           weaver_L.Weave(asm);
        }

        

        [Test]
        public void CheckBefore()
        {
            var res = new MyClassToWeave().CheckBefore(new BeforeParameter {Value = "not before"});
            Assert.AreEqual("Value set in before", res);
        }

        [Test]
        public void CheckStatic()
        {
            var res = MyClassToWeave.CheckStatic(new BeforeParameter { Value = "not before" });
            Assert.AreEqual("Value set in before", res);
        }

        [Test]
        public void CheckNotRenameInAssembly()
        {
            var res = new MyClassToWeave().CheckNotRenameInAssembly();
            Assert.AreEqual("Weaved", res);
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
        public void CheckWithGenerics()
        {
            var res = new MyClassToWeave().CheckWithGenerics("Weaved");
           Assert.AreEqual("Weaved<>System.StringWeaved", res);
        }

        [Test]
        public void CheckWithParameters()
        {
            var res = new MyClassToWeave().CheckWithParameters("Weaved with parameters");
            Assert.AreEqual("Weaved with parameters", res);
        }

        [Test]
        public void CheckWithReturn()
        {
            var res = new MyClassToWeave().CheckWithReturn();
            Assert.AreEqual("Weaved", res);
        }

        [Test]
        public void CheckWithVoid()
        {
           new MyClassToWeave().CheckWithVoid();
        }

        [Test]
        public void CheckWithAttributes()
        {
           var res = new MyClassToWeaveWithAttributes().CheckBefore(new BeforeParameter { Value = "not before" });
           Assert.AreEqual("Value set in before", res);
        }
    }
}