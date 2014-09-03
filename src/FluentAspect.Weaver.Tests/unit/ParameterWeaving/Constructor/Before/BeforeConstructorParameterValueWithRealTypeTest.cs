using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterValueWithRealTypeTest :
      NetAspectTest<BeforeConstructorParameterValueWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.ParameterValue);
            var classToWeave_L = new ClassToWeave("value");

            Assert.AreEqual("value", MyAspect.ParameterValue);
         };
      }

      public class ClassToWeave
      {
         public ClassToWeave([MyAspect] string p)
         {
            p = "OtherValue";
         }
      }

      public class MyAspect : Attribute
      {
         public static string ParameterValue;
         public bool NetAspectAttribute = true;

         public void BeforeConstructorForParameter(string parameterValue)
         {
            ParameterValue = parameterValue;
         }
      }
   }
}
