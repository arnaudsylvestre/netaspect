using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Parameter
{
   public class ParameterValueParameterWithRealTypeTest :
      NetAspectTest<ParameterValueParameterWithRealTypeTest.ClassToWeave>
   {
       public ParameterValueParameterWithRealTypeTest()
            : base("It can be declared with the same type as the parameter type", "ConstructorWeavingBefore", "ConstructorWeaving")
      {
      }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspectAttribute.ParameterValue);
            var classToWeave_L = new ClassToWeave("value");

            Assert.AreEqual("OtherValue", MyAspectAttribute.ParameterValue);
         };
      }

      public class ClassToWeave
      {
         public ClassToWeave([MyAspect] string p)
         {
            p = "OtherValue";
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static string ParameterValue;
         public bool NetAspectAttribute = true;

         public void AfterConstructorForParameter(string parameterValue)
         {
            ParameterValue = parameterValue;
         }
      }
   }
}
