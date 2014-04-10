using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.CallerParameterName
{
    public class BeforeCallMethodCallerParameterNameParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallMethodCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
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

            public string Weaved(int param1)
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(object callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}