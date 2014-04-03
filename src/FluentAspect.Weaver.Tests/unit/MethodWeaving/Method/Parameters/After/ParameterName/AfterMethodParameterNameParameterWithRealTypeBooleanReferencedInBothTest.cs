using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeBooleanReferencedInBothTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeBooleanReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(false, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    bool b = true;
                    classToWeave_L.Weaved(ref b);
                    Assert.AreEqual(true, MyAspect.I);
                    Assert.AreEqual(false, b);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref Boolean i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Boolean I;
            public bool NetAspectAttribute = true;

            public void After(ref Boolean i)
            {
                I = i;
                i = false;
            }
        }
    }
}