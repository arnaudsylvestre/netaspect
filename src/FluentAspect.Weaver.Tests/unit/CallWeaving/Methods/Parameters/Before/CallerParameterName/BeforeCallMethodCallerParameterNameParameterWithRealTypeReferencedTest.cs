using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.CallerParameterName
{
    public class BeforeCallMethodCallerParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<BeforeCallMethodCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual(10, classToWeave_L.Weaved(12));
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public int Weaved(int param1)
            {
                Method();
                return param1;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(ref int callerParam1)
            {
                ParameterName = callerParam1;
                callerParam1 = 10;
            }
        }
    }
}