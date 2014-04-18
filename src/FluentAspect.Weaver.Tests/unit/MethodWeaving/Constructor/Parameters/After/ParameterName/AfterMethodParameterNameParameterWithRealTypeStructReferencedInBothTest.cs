using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeStructReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeStructReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.AreEqual(0, MyAspect.I.A);
                   var testStruct_L = new TestStruct { A = 12 };
                    var classToWeave_L = new ClassToWeave(ref testStruct_L);
                    Assert.AreEqual(12, MyAspect.I.A);
                    Assert.AreEqual(55, testStruct_L.A);
                };
        }

        public struct TestStruct
        {
            public int A;
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref TestStruct i)
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
                i = new TestStruct
                    {
                        A = 55
                    };
            }
        }
    }
}