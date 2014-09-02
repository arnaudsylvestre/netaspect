using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeInt64ReferencedInBothTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeInt64ReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    Int64 i = 12;
                    classToWeave_L.Weaved(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(64, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref Int64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Int64 I;
            public bool NetAspectAttribute = true;

            public void After(ref Int64 i)
            {
                I = i;
                i = 64;
            }
        }
    }
}