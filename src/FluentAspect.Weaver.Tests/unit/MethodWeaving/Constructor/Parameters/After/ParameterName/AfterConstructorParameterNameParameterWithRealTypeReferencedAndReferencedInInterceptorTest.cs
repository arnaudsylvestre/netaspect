using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeReferencedAndReferencedInInterceptorTest :
        NetAspectTest
            <AfterConstructorParameterNameParameterWithRealTypeReferencedAndReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    int value = 12;
                    var classToWeave_L = new ClassToWeave(ref value);
                    Assert.AreEqual(8, classToWeave_L.I);
                    Assert.AreEqual(8, MyAspect.I);
                    Assert.AreEqual(5, value);
                };
        }

        public class ClassToWeave
        {
            public int I;

            [MyAspect]
            public ClassToWeave(ref int i)
            {
                i = 8;
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
                i = 5;
            }
        }
    }
}