using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.ParameterWeaving
{
   public class Part8Sample1BeforeParameterPossibilityTest :
      NetAspectTest<Part8Sample1BeforeParameterPossibilityTest.MyInt>
   {
      public Part8Sample1BeforeParameterPossibilityTest()
         : base("Before parameter", "ParameterWeavingBefore", "ParameterWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new MyInt(12);
            classToWeave_L.DivideBy(6);
            Assert.True(LogAttribute.Called);
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

         public void BeforeMethodForParameter(int parameterValue,
            string parameterName,
            MyInt instance,
            int v,
            object[] parameters,
            ParameterInfo parameter)
         {
            Called = true;
            Assert.NotNull(instance);
            Assert.AreEqual("v", parameter.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("v", parameterName);
            Assert.AreEqual(6, parameterValue);
            Assert.AreEqual(6, v);
         }
      }
   }
}
