using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeShortReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeShortReferencedInInterceptorTest.ClassToWeave>
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
            public ClassToWeave(short i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static short I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref short i)
            {
                I = i;
            }
        }
    }
}