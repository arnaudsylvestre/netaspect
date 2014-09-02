using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Parameters.ParameterValue
{
   public class AfterConstructorParameterValueWithRealTypeReferencedTest :
        NetAspectTest<AfterConstructorParameterValueWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                   var classToWeave_L = new ClassToWeave("value");
                    
                    Assert.AreEqual("OtherValue", MyAspect.ParameterValue);
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

            public void AfterConstructorForParameter(ref string parameterValue)
            {
               ParameterValue = parameterValue;
               parameterValue = "ModifiedByAspect";
            }
        }
    }
}