using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.CalledParameters
{
    public class AfterCallMethodCalledParametersParameterWithRealTypeTest :
        NetAspectTest<AfterCallMethodCalledParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CalledParameters);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CalledParameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method(int param1, int param2)
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method(1, 2);
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CalledParameters;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(object[] calledParameters)
            {
                CalledParameters = calledParameters;
            }
        }
    }
}