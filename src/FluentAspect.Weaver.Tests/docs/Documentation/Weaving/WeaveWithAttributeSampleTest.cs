using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Weaving
{
   public class WeaveWithAttributeSampleTest : NetAspectTest<WeaveWithAttributeSampleTest.MyInt>
   {
      public WeaveWithAttributeSampleTest()
         : base("This method is executed before the code of the method is executed", "MethodWeavingBefore", "MethodWeaving")
      {
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

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var myInt = new MyInt(24);
            Assert.AreEqual(2, myInt.DivideBy(12));
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void Before()
         {
            Called = true;
         }
      }
   }
}
