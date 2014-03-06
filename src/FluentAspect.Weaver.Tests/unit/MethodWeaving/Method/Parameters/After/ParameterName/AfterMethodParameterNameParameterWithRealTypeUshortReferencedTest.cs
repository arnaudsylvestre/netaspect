using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeUshortReferencedTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeUshortReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    ushort i_L = 12;
                    classToWeave_L.Weaved(ref i_L);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref ushort i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static ushort I;
            public bool NetAspectAttribute = true;

            public void After(ushort i)
            {
                I = i;
            }
        }
    }
}