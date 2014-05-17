using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterValueWithRealTypeReferencedTest :
        NetAspectTest<BeforeConstructorParameterValueWithRealTypeReferencedTest.ClassToWeave>
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
               
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterValue;
            public bool NetAspectAttribute = true;

            public void BeforeConstructorForParameter(ref string parameterValue)
            {
               ParameterValue = parameterValue;
               parameterValue = "ModifiedByAspect";
            }
        }
    }
}