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

        private static readonly object myClassToWeave = Weave();

        private static object WeaveAndCheck(string checkwithreturn_L, object[] parameters)
        {
            return myClassToWeave.GetType().GetMethod(checkwithreturn_L).Invoke(myClassToWeave, parameters);
        }

        [Test, Ignore]
        public void LaunchWeaving()
        {
            const string asm = "FluentAspect.Sample.exe";
            const string dst = "FluentAspect.Sample.exe";
            var weaver_L = new WeaverTool(asm, dst);
            weaver_L.Weave();
        }

        private static object Weave()
        {
            
            const string dst = "FluentAspect.Sample.exe";
            Type myClassToWeaveType =
                (from t in Assembly.LoadFrom(dst).GetTypes() where t.Name == "MyClassToWeave" select t).First();
            return Activator.CreateInstance(myClassToWeaveType);
        }

        private static object WeaveAndCheck<T>(string checkwithreturn_L, object[] parameters)
        {
            object res =
                myClassToWeave.GetType()
                          .GetMethod(checkwithreturn_L)
                          .MakeGenericMethod(typeof (T))
                          .Invoke(myClassToWeave, parameters);
            return res;
        }

        [Test]
        public void CheckBefore()
        {
            object res = WeaveAndCheck("CheckBefore", new object[] { new BeforeParameter { Value = "not before" } });
            Assert.AreEqual("Value set in before", res);
        }

        [Test]
        public void CheckNotRenameInAssembly()
        {
            object res = WeaveAndCheck("CheckNotRenameInAssembly", new object[] {});
            Assert.AreEqual("Weaved", res);
        }

        [Test, ExpectedException(typeof (NotSupportedException))]
        public void CheckThrow()
        {
            try
            {
                WeaveAndCheck("CheckThrow", new object[] {});
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [Test]
        public void CheckWithGenerics()
        {
           object res = WeaveAndCheck<string>("CheckWithGenerics", new object[] { "Weaved" });
           Assert.AreEqual("Weaved<>System.StringWeaved", res);
        }

        [Test]
        public void CheckWithParameters()
        {
            object res = WeaveAndCheck("CheckWithParameters", new object[] {"Weaved with parameters"});
            Assert.AreEqual("Weaved with parameters", res);
        }

        [Test]
        public void CheckWithReturn()
        {
            object res = WeaveAndCheck("CheckWithReturn", new object[0]);
            Assert.AreEqual("Weaved", res);
        }

        [Test]
        public void CheckWithVoid()
        {
            WeaveAndCheck("CheckWithVoid", new object[] {});
        }
    }
}