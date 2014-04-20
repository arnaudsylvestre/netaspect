using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeEnumReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeEnumReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(TestEnum.A, MyAspect.I);
                    var testEnum = TestEnum.B;
                    var classToWeave_L = new ClassToWeave(ref testEnum);
                    Assert.AreEqual(TestEnum.B, MyAspect.I);
                    Assert.AreEqual(TestEnum.C, testEnum);
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
            public ClassToWeave(ref TestEnum i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static TestEnum I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref TestEnum i)
            {
                I = i;
                i = TestEnum.C;
            }
        }
    }
}