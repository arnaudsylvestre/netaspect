using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeStructReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeStructReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I.A);
                    var classToWeave_L = new ClassToWeave(new TestStruct {A = 12});
                    Assert.AreEqual(12, MyAspect.I.A);
                };
        }

        public struct TestStruct
        {
            public int A;
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(TestStruct i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static TestStruct I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref TestStruct i)
            {
                I = i;
            }
        }
    }
}