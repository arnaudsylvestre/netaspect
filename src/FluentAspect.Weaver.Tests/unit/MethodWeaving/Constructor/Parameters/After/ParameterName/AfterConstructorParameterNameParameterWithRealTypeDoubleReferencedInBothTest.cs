using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeDoubleReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeDoubleReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    double d = 2.3;
                    var classToWeave_L = new ClassToWeave(ref d);
                    Assert.AreEqual(2.3, MyAspect.I);
                    Assert.AreEqual(12, d);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref Double i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Double I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref Double i)
            {
                I = i;
                i = 12;
            }
        }
    }
}