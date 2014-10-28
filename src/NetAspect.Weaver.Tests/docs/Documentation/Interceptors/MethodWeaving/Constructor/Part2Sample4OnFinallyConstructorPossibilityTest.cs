using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.Constructor
{
   public class Part2Sample4OnFinallyConstructorPossibilityTest : NetAspectTest<Part2Sample4OnFinallyConstructorPossibilityTest.MyInt>
   {
      public Part2Sample4OnFinallyConstructorPossibilityTest()
         : base("This method is executed after the constructor is executed if an exception is raised or not", "ConstructorWeavingOnFinally", "ConstructorWeaving")
      {
      }

      public class MyInt
      {
         private readonly int value;

         [Log]
         public MyInt(int intValue)
         {
            value = intValue;
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

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var myInt = new MyInt(24);
            myInt.DivideBy(12);
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnFinallyConstructor(object instance, MethodBase constructor, object[] parameters, int intValue, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(24, intValue);
            Assert.AreEqual(19, lineNumber);
            Assert.AreEqual(10, columnNumber);
            Assert.AreEqual("Part2Sample4OnFinallyConstructorPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"Constructor\Part2Sample4OnFinallyConstructorPossibilityTest.cs"));
         }
      }
   }
}
