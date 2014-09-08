using System;
using System.Text;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
   public class MethodWeavingForDocWithStartDivisionTest :
      NetAspectTest<MethodWeavingForDocWithStartDivisionTest.MyInt>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            LogAttribute.Console = new StringBuilder();
            var value = new MyInt(6);
            value.DivideBy(3);
            Assert.AreEqual("Start Division", LogAttribute.Console.ToString());
         };
      }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         public int Value
         {
            get { return value; }
         }

         [Log]
         public int DivideBy(int v)
         {
            return value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static StringBuilder Console;
         public bool NetAspectAttribute = true;

         public void BeforeMethod()
         {
            Console.Append("Start Division");
         }
      }
   }
}
