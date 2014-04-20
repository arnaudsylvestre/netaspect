using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeBooleanReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeBooleanReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(false, MyAspect.I);
                    bool b = true;
                    var classToWeave_L = new ClassToWeave(ref b);
                    Assert.AreEqual(true, MyAspect.I);
                    Assert.AreEqual(false, b);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref Boolean i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Boolean I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref Boolean i)
            {
                I = i;
                i = false;
            }
        }
    }
}