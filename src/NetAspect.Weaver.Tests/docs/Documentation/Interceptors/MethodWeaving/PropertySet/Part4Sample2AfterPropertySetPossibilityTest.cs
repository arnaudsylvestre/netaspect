using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.PropertySet
{
   public class Part4Sample2AfterPropertySetPossibilityTest : NetAspectTest<Part4Sample2AfterPropertySetPossibilityTest.MyInt>
   {
      public Part4Sample2AfterPropertySetPossibilityTest()
         : base("After Property Setter Weaving possibilities", "PropertySetWeavingAfter", "PropertySetWeaving")
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
            myInt.Value = 32;
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(object instance, PropertyInfo property, int propertyValue, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(32, propertyValue);
            Assert.AreEqual(27, lineNumber);
            Assert.AreEqual(17, columnNumber);
            Assert.AreEqual("Part4Sample2AfterPropertySetPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"PropertySet\Part4Sample2AfterPropertySetPossibilityTest.cs"));
         }
      }
   }
}
