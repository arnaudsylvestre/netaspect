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
            };
      }
   }
}
