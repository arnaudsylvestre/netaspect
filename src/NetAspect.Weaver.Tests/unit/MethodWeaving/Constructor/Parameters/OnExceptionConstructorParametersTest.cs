using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters
{
   public class OnExceptionConstructorParametersTest : NetAspectTest<OnExceptionConstructorParametersTest.MyInt>
   {
      public OnExceptionConstructorParametersTest()
         : base("This method is executed when an exception occurs when the constructor is executed", "ConstructorWeavingOnException", "ConstructorWeaving")
      {
      }

      public class MyInt
      {
         private readonly int value;

         [Log]
         public MyInt(int intValue)
         {
            if (value == 0)
               throw new NotSupportedException();
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
            try
            {
               new MyInt(0);
               Assert.Fail("Must raise an exception");
            }
            catch (NotSupportedException)
            {
            }
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnExceptionConstructor(object instance, ConstructorInfo constructor, object[] parameters, int intValue, Exception exception, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(0, intValue);
            Assert.AreEqual("NotSupportedException", exception.GetType().Name);
         }
      }
   }
}
