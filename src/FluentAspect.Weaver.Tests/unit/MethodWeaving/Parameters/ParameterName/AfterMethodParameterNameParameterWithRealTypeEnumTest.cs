using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeEnumTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeEnumTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(TestEnum.A, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(TestEnum.B);
                    Assert.AreEqual(TestEnum.B, MyAspect.I);
                };
        }

        public enum TestEnum
        {
            A,
            B,
            C,
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(TestEnum i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static TestEnum I;
            public bool NetAspectAttribute = true;

            public void After(TestEnum i)
            {
                I = i;
            }
        }
    }
}