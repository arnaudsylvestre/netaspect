using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeDoubleTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeDoubleTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave(2.3);
                    Assert.AreEqual(2.3, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(Double i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Double I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(Double i)
            {
                I = i;
            }
        }
    }
}