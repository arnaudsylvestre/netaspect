using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.CallerParameters
{
    public class AfterCallGetFieldCallerParametersParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetFieldCallerParametersParameterWithRealTypeTest.ClassToWeave>
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

            public void AfterGetField(object[] callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}