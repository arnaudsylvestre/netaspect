using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.CalledParameterName
{
    public class BeforeCallMethodCalledParameterNameParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallMethodCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
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

            public void BeforeCallMethod(object calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}