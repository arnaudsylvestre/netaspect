using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeInt64ReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeInt64ReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    Int64 i = 12; 
                   var classToWeave_L = new ClassToWeave(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(64, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref Int64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Int64 I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref Int64 i)
            {
                I = i;
                i = 64;
            }
        }
    }
}