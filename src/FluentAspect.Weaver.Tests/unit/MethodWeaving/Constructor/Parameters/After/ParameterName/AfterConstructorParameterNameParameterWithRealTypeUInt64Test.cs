using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeUInt64Test :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeUInt64Test.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(UInt64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static UInt64 I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(UInt64 i)
            {
                I = i;
            }
        }
    }
}