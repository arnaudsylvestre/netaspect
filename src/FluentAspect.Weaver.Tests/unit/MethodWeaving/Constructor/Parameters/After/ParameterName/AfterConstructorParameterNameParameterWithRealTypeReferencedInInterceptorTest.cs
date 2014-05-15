using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave(12);
                    Assert.AreEqual(12, classToWeave_L.I);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            public int I;

            [MyAspect]
            public ClassToWeave(int i)
            {
                I = i;
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref int i)
            {
                I = i;
                i = 3;
            }
        }
    }
}