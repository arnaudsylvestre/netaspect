using System;
using System.Linq;
using System.Reflection;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverToolTest
    {
        //public void Sample()
        //{
        //    object[] args = new object[0];
        //    Around.Call(this, "CheckWith", args, new CheckThrowInterceptor());
        //}

        public string Sample<U>(U u)
        {
            var interceptor = new CheckThrowInterceptor();
            var args = new object[0];
            MethodInfo method = GetType().GetMethod("Sample");
            var methodCall = new MethodCall(this, method, args);
            string weavedResult;
            try
            {
                interceptor.Before(methodCall);
                string result = SampleWeaved<U>(u);
                var methodCallResult = new MethodCallResult(result);
                interceptor.After(methodCall, methodCallResult);
                weavedResult = (string) methodCallResult.Result;
            }
            catch (Exception e)
            {
                var ex = new ExceptionResult(e);
                interceptor.OnException(methodCall, ex);
                object cancelExceptionAndReturn = ex.CancelExceptionAndReturn;
                if (cancelExceptionAndReturn == null)
                    throw;
                weavedResult = (string) cancelExceptionAndReturn;
            }
            return weavedResult;
        }

        public string SampleWeaved<T>(T t)
        {
            return "Not Weaved";
        }


        [Test, Ignore]
        public void LaunchWeaving()
        {
            const string asm = "FluentAspect.Sample.exe";
            const string dst = "FluentAspect.Sample.exe";
            var weaver_L = new WeaverTool(asm, dst);
            weaver_L.Weave();
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
    }
}