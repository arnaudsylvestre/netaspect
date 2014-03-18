using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After
{
    public class AfterConstructorParameterNameParameterWithRealTypeTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeTest.ClassToWeave>
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
            public ClassToWeave(int i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void After(int i)
            {
                I = i;
            }
        }
    }
}