using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.Constructor
{
   public class Part2Sample1BeforeConstructorPossibilityTest : NetAspectTest<Part2Sample1BeforeConstructorPossibilityTest.MyInt>
   {
      public Part2Sample1BeforeConstructorPossibilityTest()
         : base("This method is executed before the code of the constructor is executed", "ConstructorWeavingBefore", "ConstructorWeaving")
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
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(object instance, MethodBase constructor, object[] parameters, int intValue, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(24, intValue);
            Assert.AreEqual(20, lineNumber);
            Assert.AreEqual(10, columnNumber);
            Assert.AreEqual("Part2Sample1BeforeConstructorPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"Constructor\Part2Sample1BeforeConstructorPossibilityTest.cs"));
         }
      }
   }
}
