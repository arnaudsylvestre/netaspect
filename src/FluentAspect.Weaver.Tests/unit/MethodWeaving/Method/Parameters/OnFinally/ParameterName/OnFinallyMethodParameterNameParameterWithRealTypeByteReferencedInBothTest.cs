using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.ParameterName
{
    public class OnFinallyMethodParameterNameParameterWithRealTypeByteReferencedInBothTest :
        NetAspectTest<OnFinallyMethodParameterNameParameterWithRealTypeByteReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    byte i = 12;
                    classToWeave_L.Weaved(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(14, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref byte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void OnFinally(ref byte i)
            {
                I = i;
                i = 14;
            }
        }
    }
}