using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.Constructor
{
   public class Part2Sample2AfterConstructorPossibilityTest : NetAspectTest<Part2Sample2AfterConstructorPossibilityTest.MyInt>
   {
      public Part2Sample2AfterConstructorPossibilityTest()
         : base("This method is executed after the code of the constructor is executed", "ConstructordWeavingAfter", "ConstructorWeaving")
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
            Assert.AreEqual(2, myInt.DivideBy(12));
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void AfterConstructor(object instance, ConstructorInfo constructor, object[] parameters, int intValue, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(24, intValue);
            Assert.AreEqual(19, lineNumber);
            Assert.AreEqual(10, columnNumber);
            Assert.AreEqual("Part2Sample2AfterConstructorPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"Constructor\Part2Sample2AfterConstructorPossibilityTest.cs"));
         }
      }
   }
}
