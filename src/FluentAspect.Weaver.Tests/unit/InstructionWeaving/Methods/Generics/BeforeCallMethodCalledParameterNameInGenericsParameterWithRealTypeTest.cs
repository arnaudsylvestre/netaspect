using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Generics
{
    public class BeforeCallMethodCalledParameterNameInGenericsParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodCalledParameterNameInGenericsParameterWithRealTypeTest.MyAspect>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave<int>();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave<T>
        {
            [MyAspect]
            public string Method(T param1)
            {
                return "Hello";
            }

            public string Weaved(T param1)
            {
                return Method(param1);
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