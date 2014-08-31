using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Generics
{
    public class BeforeCallMethodCalledParameterNameGenericsParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodCalledParameterNameGenericsParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method<T>(T param1)
            {
                return "Hello";
            }

            public string Weaved()
            {
                int toto = 12;
                return Method(toto);
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