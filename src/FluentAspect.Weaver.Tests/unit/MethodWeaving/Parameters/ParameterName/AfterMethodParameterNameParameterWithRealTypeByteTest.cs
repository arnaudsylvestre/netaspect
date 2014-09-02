using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeByteTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeByteTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(byte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void After(byte i)
            {
                I = i;
            }
        }
    }
}