using System;
using FluentAspect.Sample;
using FluentAspect.Sample.AOP;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Interceptors
{
   [TestFixture]
   public class BeforeAcceptanceTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               MockInterceptorAttribute.after = null;
               MockInterceptorAttribute.before = null;
               MockInterceptorAttribute.exception = null;
               var myClassToWeave = new MyClassToWeave();
               string res = myClassToWeave.CheckMock("param");
               Assert.AreSame(myClassToWeave, MockInterceptorAttribute.before.@this);
               Assert.AreEqual("CheckMock", MockInterceptorAttribute.before.methodInfo_P.Name);
               Assert.AreEqual(new object[] { "param" }, MockInterceptorAttribute.before.parameters);
               Assert.AreSame(myClassToWeave, MockInterceptorAttribute.after.@this);
               Assert.AreEqual("CheckMock", MockInterceptorAttribute.after.methodInfo_P.Name);
               Assert.AreEqual(new object[] { "param" }, MockInterceptorAttribute.after.parameters);
               Assert.AreEqual(res, MockInterceptorAttribute.after.result);
               Assert.AreEqual(null, MockInterceptorAttribute.exception);
            };
      }
   }
}
