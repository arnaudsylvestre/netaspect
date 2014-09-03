using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.MethodWeaving.Method
{
   public class GettingStartedPart1Test : NetAspectTest<GettingStartedPart1Test.Computer>
   {
      public class Computer
      {
         public int Divide(int a, int b)
         {
            return a / b;
         }
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var computer = new Computer();
            Assert.AreEqual(2, computer.Divide(12, 6));
         };
      }
   }
}
