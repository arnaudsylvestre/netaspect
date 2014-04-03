using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.CalledParameters
{
    public class BeforeCallMethodCalledParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodCalledParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CalledParameters);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(1, 2);
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CalledParameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved(int param1, int param2)
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CalledParameters;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(object[] calledParameters)
            {
                CalledParameters = calledParameters;
            }
        }
    }
}