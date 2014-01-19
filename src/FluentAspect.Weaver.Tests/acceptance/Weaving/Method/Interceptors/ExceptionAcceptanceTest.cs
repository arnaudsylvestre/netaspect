using System;
using FluentAspect.Sample;
using FluentAspect.Sample.AOP;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Interceptors
{
    [TestFixture]
    public class ExceptionAcceptanceTest : AcceptanceTest
    {
        protected override Action Execute()
        {
            return () =>
            {
                throw new NotImplementedException();
                    MockInterceptorAttribute.after = null;
                    MockInterceptorAttribute.before = null;
                    MockInterceptorAttribute.exception = null;
                    var myClassToWeave = new MyClassToWeave();
                    try
                    {
                        string res = myClassToWeave.CheckMockException();
                    }
                    catch (NotImplementedException)
                    {
                        Assert.AreSame(myClassToWeave, MockInterceptorAttribute.before.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptorAttribute.before.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptorAttribute.before.parameters);
                        Assert.AreSame(myClassToWeave, MockInterceptorAttribute.exception.@this);
                        Assert.AreEqual("CheckMockException", MockInterceptorAttribute.exception.methodInfo_P.Name);
                        Assert.AreEqual(new object[0], MockInterceptorAttribute.exception.parameters);
                        Assert.True(MockInterceptorAttribute.exception.Exception is NotImplementedException);
                    }
                };
        }
    }
}