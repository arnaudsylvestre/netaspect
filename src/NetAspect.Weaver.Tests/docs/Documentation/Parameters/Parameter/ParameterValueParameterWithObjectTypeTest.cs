using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Parameter
{
   public class ParameterValueParameterWithObjectTypeTest :
      NetAspectTest<ParameterValueParameterWithObjectTypeTest.ClassToWeave>
   {

       
       public ParameterValueParameterWithObjectTypeTest()
           : base("It can be declared as object", "ConstructorWeavingBefore", "ConstructorWeaving")
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
         public static object ParameterValue;
         public bool NetAspectAttribute = true;

         public void AfterConstructorForParameter(object parameterValue)
         {
            ParameterValue = parameterValue;
         }
      }
   }
}
