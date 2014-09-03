using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Weaving
{
   public class WeaveWithSelectSampleTest : NetAspectTest<WeaveWithSelectSampleTest.MyInt>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var myInt = new MyInt(24);
            Assert.AreEqual(2, myInt.DivideBy(12));
            Assert.True(LogAttribute.Called);
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

         public int DivideBy(int v)
         {
            return value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void Before()
         {
            Called = true;
         }

         public static bool SelectMethod(MethodInfo method)
         {
            return method.Name == "DivideBy";
         }
      }
   }
}
