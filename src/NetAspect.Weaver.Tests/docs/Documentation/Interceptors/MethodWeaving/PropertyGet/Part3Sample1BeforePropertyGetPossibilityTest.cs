using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.PropertyGet
{
   public class Part3Sample1BeforePropertyGetPossibilityTest : NetAspectTest<Part3Sample1BeforePropertyGetPossibilityTest.MyInt>
   {
      public Part3Sample1BeforePropertyGetPossibilityTest()
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

         public void BeforePropertyGetMethod(object instance, PropertyInfo property, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(28, lineNumber);
            Assert.AreEqual(17, columnNumber);
            Assert.AreEqual("Part3Sample1BeforePropertyGetPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample1BeforePropertyGetPossibilityTest.cs"));
         }
      }
   }
}
