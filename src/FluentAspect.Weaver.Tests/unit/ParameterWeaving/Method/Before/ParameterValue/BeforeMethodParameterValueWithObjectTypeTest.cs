using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeMethodParameterValueWithObjectTypeTest :
        NetAspectTest<BeforeMethodParameterValueWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved("value");
                    Assert.AreEqual("value", MyAspect.ParameterValue);
                };
        }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string p)
            {
               p = "OtherValue";
            }
        }

        public class MyAspect : Attribute
        {
           public static object ParameterValue;
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(object parameterValue)
            {
               ParameterValue = parameterValue;
            }
        }
    }
}