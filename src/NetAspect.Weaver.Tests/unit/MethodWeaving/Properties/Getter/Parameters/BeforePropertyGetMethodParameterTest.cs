using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters
{
   public class BeforePropertyGetMethodParameterTest : NetAspectTest<BeforePropertyGetMethodParameterTest.MyInt>
   {
      public BeforePropertyGetMethodParameterTest()
         : base("Before Property Getter possibilities", "PropertyGetWeavingBefore", "PropertyGetWeaving")
      {
      }


      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int Value
         {
            get { return value; }
         }

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
            Assert.AreEqual(24, myInt.Value);
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforePropertyGetMethod(object instance, PropertyInfo property, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
         }
      }
   }
}
