using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
   public class InstructionWeavingForDocTest :
      NetAspectTest<InstructionWeavingForDocTest.MyInt>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Computer.Divide(6, 3);
            Assert.AreEqual(44, LogAttribute.LineNumber);
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

      public class Computer
      {
         public static int Divide(int a, int b)
         {
            var myInt = new MyInt(a);
            return myInt.DivideBy(b);
         }
      }

      public class LogAttribute : Attribute
      {
         public static int LineNumber;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(int lineNumber)
         {
            LineNumber = lineNumber;
         }
      }
   }
}
