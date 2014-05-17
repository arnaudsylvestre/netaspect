using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
    public class BeforeConstructorParameterNameWithObjectTypeTest :
        NetAspectTest<BeforeConstructorParameterNameWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterName);
                   var classToWeave_L = new ClassToWeave("value");
                    Assert.AreEqual("p", MyAspect.ParameterName);
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
           public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeConstructorForParameter(object parameterName)
            {
               ParameterName = parameterName;
            }
        }
    }
}