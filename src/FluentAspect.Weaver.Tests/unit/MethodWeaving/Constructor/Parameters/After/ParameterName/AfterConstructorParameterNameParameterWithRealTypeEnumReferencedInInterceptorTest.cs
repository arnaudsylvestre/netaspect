using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeEnumReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeEnumReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(TestEnum.A, MyAspect.I);
                    var testEnum = TestEnum.B; 
                   var classToWeave_L = new ClassToWeave(testEnum);
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
            public ClassToWeave(TestEnum i)
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
            }
        }
    }
}