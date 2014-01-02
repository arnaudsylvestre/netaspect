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
            };
      }
   }
}
