using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters
{
   public class OnFinallyPropertySetMethodParametersTest : NetAspectTest<OnFinallyPropertySetMethodParametersTest.MyInt>
   {
      public OnFinallyPropertySetMethodParametersTest()
         : base("On finally property setter weaving possibilities", "PropertySetWeavingOnFinally", "PropertySetWeaving")
      {
      }

      public class MyInt
      {
         private int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int Value
         {
            set { this.value = value; }
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
            myInt.Value = 12;
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnFinallyPropertySetMethod(object instance, PropertyInfo property, int propertyValue, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(12, propertyValue);
         }
      }
   }
}
