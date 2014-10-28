using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.PropertySet
{
   public class Part4Sample4OnFinallyPropertySetPossibilityTest : NetAspectTest<Part4Sample4OnFinallyPropertySetPossibilityTest.MyInt>
   {
      public Part4Sample4OnFinallyPropertySetPossibilityTest()
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

         public void OnFinallyPropertySetMethod(object instance, PropertyInfo property, int propertyValue, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(12, propertyValue);
            Assert.AreEqual(27, lineNumber);
            Assert.AreEqual(17, columnNumber);
            Assert.AreEqual("Part4Sample4OnFinallyPropertySetPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"PropertySet\Part4Sample4OnFinallyPropertySetPossibilityTest.cs"));
         }
      }
   }
}
