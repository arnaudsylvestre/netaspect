using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.Method
{
   public class Part1Sample2AfterMethodPossibilityTest : NetAspectTest<Part1Sample2AfterMethodPossibilityTest.MyInt>
   {
      public Part1Sample2AfterMethodPossibilityTest()
         : base("This method is executed after the code of the method is executed", "MethodWeavingAfter", "MethodWeaving")
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

         public void AfterMethod(object instance, MethodInfo method, object[] parameters, int v, int result, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("DivideBy", method.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(12, v);
            Assert.AreEqual(2, result);
            Assert.AreEqual(31, lineNumber);
            Assert.AreEqual(10, columnNumber);
            Assert.AreEqual("Part1Sample2AfterMethodPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"Method\Part1Sample2AfterMethodPossibilityTest.cs"));
         }
      }
   }
}
