using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.CallerParameters
{
    public class BeforeCallGetFieldCallerParametersParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetFieldCallerParametersParameterWithRealTypeTest.ClassToWeave>
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
            [MyAspect] public string Field;

            public string Weaved(int param1, int param2)
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CallerParameters;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(object[] callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}