using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Method.Parameters
{
   public class BeforeMethodForParameterParametersTest :
      NetAspectTest<BeforeMethodForParameterParametersTest.MyInt>
   {
      public BeforeMethodForParameterParametersTest()
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
            MyInt instance,
            object[] parameters,
            ParameterInfo parameter)
         {
            Called = true;
            Assert.NotNull(instance);
            Assert.AreEqual("v", parameter.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(6, parameterValue);
         }
      }
   }
}
