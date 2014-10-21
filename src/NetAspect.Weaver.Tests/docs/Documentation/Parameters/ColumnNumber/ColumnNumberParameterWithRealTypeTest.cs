using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.ColumnNumber
{
   public class ColumnNumberParameterWithRealTypeTest : NetAspectTest<ColumnNumberParameterWithRealTypeTest.MyInt>
   {
       public ColumnNumberParameterWithRealTypeTest()
           : base("It must be declared with the System.Int32 type", "MethodWeavingBefore", "MethodWeaving")
       {
           
       }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            Computer.Divide(6, 3);
            Assert.AreEqual(45, LogAttribute.ColumnNumber);
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
         public static int ColumnNumber;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(int columnNumber)
         {
            ColumnNumber = columnNumber;
         }
      }
   }
}
