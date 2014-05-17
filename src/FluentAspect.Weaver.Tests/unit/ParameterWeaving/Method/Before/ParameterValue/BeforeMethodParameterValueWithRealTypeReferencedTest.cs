using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeMethodParameterValueWithRealTypeReferencedTest :
        NetAspectTest<BeforeMethodParameterValueWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual("ModifiedByAspect", classToWeave_L.Weaved("value"));
                    Assert.AreEqual("value", MyAspect.ParameterValue);
                };
        }

        public class ClassToWeave
        {
            
            public string Weaved([MyAspect] string p)
            {
               
               return p;
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterValue;
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(ref string parameterValue)
            {
               ParameterValue = parameterValue;
               parameterValue = "ModifiedByAspect";
            }
        }
    }
}