using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterMethodParameterValueWithRealTypeReferencedTest :
        NetAspectTest<AfterMethodParameterValueWithRealTypeReferencedTest.ClassToWeave>
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
            
            public string Weaved([MyAspect] string parameter)
            {
               parameter = "OtherValue";
               return parameter;
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterValue;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(ref string parameterValue)
            {
               ParameterValue = parameterValue;
               parameterValue = "ModifiedByAspect";
            }
        }
    }
}