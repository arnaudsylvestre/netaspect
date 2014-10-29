using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Members
{
   public class AspectParameterInConstructorTest : NetAspectTest<AspectParameterInConstructorTest.MyInt>
   {

      public class MyInt
      {
         private readonly int value;

         [Log(10)]
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

          public int max;

          public LogAttribute(int max)
          {
              this.max = max;
          }

          public void AfterConstructor(object instance, ConstructorInfo constructor, object[] parameters, int intValue, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(24, intValue);
            Assert.AreEqual(10, max);
         }
      }
   }
}
