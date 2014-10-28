using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.MethodWeaving.PropertyGet
{
   public class Part3Sample3OnExceptionPropertyGetPossibilityTest : NetAspectTest<Part3Sample3OnExceptionPropertyGetPossibilityTest.MyInt>
   {
      public Part3Sample3OnExceptionPropertyGetPossibilityTest()
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

         public void OnExceptionPropertyGetMethod(object instance, PropertyInfo property, Exception exception, int lineNumber, int columnNumber, string fileName, string filePath)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual("NotSupportedException", exception.GetType().Name);
            Assert.AreEqual(28, lineNumber);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual("Part3Sample3OnExceptionPropertyGetPossibilityTest.cs", fileName);
            Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample3OnExceptionPropertyGetPossibilityTest.cs"));
         }
      }
   }
}
