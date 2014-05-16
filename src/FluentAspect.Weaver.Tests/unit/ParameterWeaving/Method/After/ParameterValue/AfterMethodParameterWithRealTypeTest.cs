using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterMethodParameterValueWithRealTypeTest :
        NetAspectTest<AfterMethodParameterValueWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved("value");
                    Assert.AreEqual("OtherValue", MyAspect.ParameterValue);
                };
        }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string parameter)
            {
               parameter = "OtherValue";
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterValue;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(string parameterValue)
            {
               ParameterValue = parameterValue;
            }
        }
    }
}