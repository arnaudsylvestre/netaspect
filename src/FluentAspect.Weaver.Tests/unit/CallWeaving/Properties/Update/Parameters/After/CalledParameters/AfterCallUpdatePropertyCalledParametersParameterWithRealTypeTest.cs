using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.CalledParameters
{
    public class AfterCallUpdatePropertyCalledParametersParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyCalledParametersParameterWithRealTypeTest.ClassToWeave>
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
            public string Property { get; set; }

            public void Weaved(int param1, int param2)
            {
                Property = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CalledParameters;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(object[] calledParameters)
            {
                CalledParameters = calledParameters;
            }
        }
    }
}