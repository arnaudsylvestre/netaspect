using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeShortReferencedTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeShortReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    short i = 12;
                    classToWeave_L.Weaved(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref short i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static short I;
            public bool NetAspectAttribute = true;

            public void After(short i)
            {
                I = i;
            }
        }
    }
}