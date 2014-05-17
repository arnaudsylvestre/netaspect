using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterConstructorParameterNameWithObjectTypeTest :
        NetAspectTest<AfterConstructorParameterNameWithObjectTypeTest.ClassToWeave>
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

            public void AfterConstructorForParameter(object parameterName)
            {
               ParameterName = parameterName;
            }
        }
    }
}