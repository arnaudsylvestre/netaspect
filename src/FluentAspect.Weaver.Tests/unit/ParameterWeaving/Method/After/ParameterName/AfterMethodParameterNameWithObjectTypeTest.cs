using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterMethodParameterNameWithObjectTypeTest :
        NetAspectTest<AfterMethodParameterNameWithObjectTypeTest.ClassToWeave>
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
           public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(object parameterName)
            {
               ParameterName = parameterName;
            }
        }
    }
}