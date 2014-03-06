using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeUInt64Test :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeUInt64Test.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(UInt64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static UInt64 I;
            public bool NetAspectAttribute = true;

            public void After(UInt64 i)
            {
                I = i;
            }
        }
    }
}