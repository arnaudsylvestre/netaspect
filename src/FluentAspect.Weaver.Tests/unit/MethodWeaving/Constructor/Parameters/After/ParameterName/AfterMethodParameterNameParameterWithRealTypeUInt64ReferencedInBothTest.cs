using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeUInt64ReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeUInt64ReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    UInt64 i_L = 12; 
                   var classToWeave_L = new ClassToWeave(ref i_L);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(55, i_L);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref UInt64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static UInt64 I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref UInt64 i)
            {
                I = i;
                i = 55;
            }
        }
    }
}