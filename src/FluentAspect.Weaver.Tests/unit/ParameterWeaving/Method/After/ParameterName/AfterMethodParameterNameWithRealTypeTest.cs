using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterMethodParameterNameWithRealTypeTest :
        NetAspectTest<AfterMethodParameterNameWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved("value");
                    Assert.AreEqual("parameter", MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string parameter)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(string parameterName)
            {
               ParameterName = parameterName;
            }
        }
    }
}