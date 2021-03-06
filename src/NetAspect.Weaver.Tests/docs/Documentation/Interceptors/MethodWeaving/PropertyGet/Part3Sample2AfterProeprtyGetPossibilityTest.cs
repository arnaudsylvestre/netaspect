using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.PropertyGet
{
   public class Part3Sample2AfterProeprtyGetPossibilityTest : NetAspectTest<Part3Sample2AfterProeprtyGetPossibilityTest.MyInt>
   {
      public Part3Sample2AfterProeprtyGetPossibilityTest()
         : base("After Property Getter Weaving possibilities", "PropertyGetWeavingAfter", "PropertyGetWeaving")
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

         public void AfterPropertyGetMethod(object instance, PropertyInfo property, int result, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(24, result);
            Assert.AreEqual(27, lineNumber);
            Assert.AreEqual(17, columnNumber);
            Assert.AreEqual("Part3Sample2AfterProeprtyGetPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample2AfterProeprtyGetPossibilityTest.cs"));
         }
      }
   }
}
