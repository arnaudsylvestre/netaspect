using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.ParameterWeaving
{
   public class Part8Sample3OnExceptionParameterPossibilityTest :
      NetAspectTest<Part8Sample3OnExceptionParameterPossibilityTest.MyInt>
   {
      public Part8Sample3OnExceptionParameterPossibilityTest()
         : base("On exception parameter", "ParameterWeavingOnException", "ParameterWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            try
            {
               var classToWeave_L = new MyInt(12);
               classToWeave_L.DivideBy(0);
               Assert.Fail("Must Fail");
            }
            catch (Exception)
            {
               Assert.True(LogAttribute.Called);
            }
         };
      }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         public int DivideBy([Log] int v)
         {
            return value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnExceptionMethodForParameter(int parameterValue,
            string parameterName,
            MyInt instance,
            int v,
            object[] parameters,
            ParameterInfo parameter,
            Exception exception)
         {
            Called = true;
            Assert.NotNull(instance);
            Assert.AreEqual("v", parameter.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("v", parameterName);
            Assert.AreEqual(0, parameterValue);
            Assert.AreEqual("DivideByZeroException", exception.GetType().Name);
            Assert.AreEqual(0, v);
         }
      }
   }
}
