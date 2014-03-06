using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeSingleReferencedTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSingleReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    float f = 3F;
                    classToWeave_L.Weaved(ref f);
                    Assert.AreEqual(3F, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref Single i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Single I;
            public bool NetAspectAttribute = true;

            public void After(Single i)
            {
                I = i;
            }
        }
    }
}