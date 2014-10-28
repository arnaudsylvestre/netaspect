using System;
using System.IO;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.GettingStarted
{
   public class GettingStartedPart2Test : NetAspectTest<GettingStartedPart2Test.Computer>
   {
      public class Computer
      {
         [Log]
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
             try
             {
                 computer.Divide(12, 0);
             }
             catch (Exception)
             {
                 Assert.AreEqual("An exception !", LogAttribute.writer.ToString());
             }
         };
      }


      public class LogAttribute : Attribute
      {
         public bool NetAspectAttribute = true;
         public static StringWriter writer = new StringWriter();


         public void OnExceptionMethod()
         {
            writer.Write("An exception !");
         }
      }
   }
}
