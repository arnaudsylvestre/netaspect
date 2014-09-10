using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Constructor.Parameters
{
   public class OnFinallyMethodForParameterParametersTest :
      NetAspectTest<OnFinallyMethodForParameterParametersTest.MyInt>
   {
      public OnFinallyMethodForParameterParametersTest()
         : base("On Finally parameter", "ParameterWeavingOnFinally", "ParameterWeaving")
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

         public void OnFinallyMethodForParameter(int parameterValue,
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
