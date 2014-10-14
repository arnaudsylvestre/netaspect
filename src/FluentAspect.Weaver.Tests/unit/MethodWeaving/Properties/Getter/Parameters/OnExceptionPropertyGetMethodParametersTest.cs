using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters
{
   public class OnExceptionPropertyGetMethodParametersTest : NetAspectTest<OnExceptionPropertyGetMethodParametersTest.MyInt>
   {
      public OnExceptionPropertyGetMethodParametersTest()
         : base("On exception property get weaving possibilities", "PropertyGetWeavingOnException", "PropertyGetWeaving")
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
            get
            {
               if (value == 0)
                  throw new NotSupportedException("Must not be 0");
               return value;
            }
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
               var myInt = new MyInt(0);
               int val = myInt.Value;
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

         public void OnExceptionPropertyGetMethod(object instance, PropertyInfo property, Exception exception, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual("NotSupportedException", exception.GetType().Name);
         }
      }
   }
}
