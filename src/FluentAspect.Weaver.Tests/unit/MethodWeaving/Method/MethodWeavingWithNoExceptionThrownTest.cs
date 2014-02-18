using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method
{
   [TestFixture]
   public class MethodWeavingWithNoExceptionThrownTest
   {
      [Test]
      public void Check()
      {
         throw new NotImplementedException("Tester le OnException sans lever d'exception");
      }
      [Test]
      public void CheckFinallyAndException()
      {
         throw new NotImplementedException("Tester le OnException et OnFinally en même temps sans lever d'exception");
      }
      [Test]
      public void CheckFinallyAndExceptionAndAfter()
      {
         throw new NotImplementedException("Tester le OnException et OnFinally et After en même temps sans lever d'exception");
      }
   }
}
