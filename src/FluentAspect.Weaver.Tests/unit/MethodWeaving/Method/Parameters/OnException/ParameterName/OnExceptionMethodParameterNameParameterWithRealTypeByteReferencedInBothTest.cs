using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.ParameterName
{
    public class OnExceptionMethodParameterNameParameterWithRealTypeByteReferencedInBothTest :
        NetAspectTest<OnExceptionMethodParameterNameParameterWithRealTypeByteReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    byte i = 12;
                    try
                    {
                        classToWeave_L.Weaved(ref i);
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(14, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref byte i)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void OnException(ref byte i)
            {
                I = i;
                i = 14;
            }
        }
    }
}