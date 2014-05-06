using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.CallerParameters
{
    public class AfterCallUpdateFieldCallerParametersParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdateFieldCallerParametersParameterWithRealTypeTest.ClassToWeave>
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

            public void Weaved(int param1, int param2)
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] CallerParameters;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(object[] callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}