using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.Method
{
   public class Part1Sample3OnExceptionMethodPossibilityTest : NetAspectTest<Part1Sample3OnExceptionMethodPossibilityTest.MyInt>
   {
      public Part1Sample3OnExceptionMethodPossibilityTest()
         : base("This method is executed when an exception occurs when the method is executed", "MethodWeavingOnException", "MethodWeaving")
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
            try
            {
               var myInt = new MyInt(24);
               myInt.DivideBy(0);
               Assert.Fail("Must raise an exception");
            }
            catch (DivideByZeroException)
            {
            }
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnExceptionMethod(object instance, MethodBase method, object[] parameters, int v, Exception exception, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("DivideBy", method.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(0, v);
            Assert.AreEqual("DivideByZeroException", exception.GetType().Name);
            Assert.AreEqual(31, lineNumber);
            Assert.AreEqual(10, columnNumber);
            Assert.AreEqual("Part1Sample3OnExceptionMethodPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"Method\Part1Sample3OnExceptionMethodPossibilityTest.cs"));
         }
      }
   }
}
