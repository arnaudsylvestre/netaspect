using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.CallerParameters
{
    public class BeforeCallGetPropertyCallerParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CallerParameters);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(1, 2);
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CallerParameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved(int param1, int param2)
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CallerParameters;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(object[] callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}