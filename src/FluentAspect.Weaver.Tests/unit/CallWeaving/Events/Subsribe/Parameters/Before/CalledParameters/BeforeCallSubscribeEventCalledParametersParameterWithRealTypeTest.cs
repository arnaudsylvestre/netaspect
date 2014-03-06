using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before.CalledParameters
{
    public class BeforeCallSubscribeEventCalledParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallSubscribeEventCalledParametersParameterWithRealTypeTest.ClassToWeave>
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
            public event Action Event;

            public void Weaved(int param1, int param2)
            {
                Event += () => { };
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CalledParameters;
            public bool NetAspectAttribute = true;

            public void BeforeRaiseEvent(object[] calledParameters)
            {
                CalledParameters = calledParameters;
            }
        }
    }
}