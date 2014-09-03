using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.MethodWeaving.Constructor
{
   public class Part2Sample3OnExceptionConstructorPossibilityTest : NetAspectTest<Part2Sample3OnExceptionConstructorPossibilityTest.MyInt>
   {
      public Part2Sample3OnExceptionConstructorPossibilityTest()
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

         public void OnExceptionConstructor(object instance, MethodBase constructor, object[] parameters, int intValue, Exception exception)
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
