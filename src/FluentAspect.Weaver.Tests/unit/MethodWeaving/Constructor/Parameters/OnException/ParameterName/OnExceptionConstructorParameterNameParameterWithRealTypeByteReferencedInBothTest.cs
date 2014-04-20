using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.ParameterName
{
    public class OnExceptionConstructorParameterNameParameterWithRealTypeByteReferencedInBothTest :
        NetAspectTest<OnExceptionConstructorParameterNameParameterWithRealTypeByteReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    byte i = 12;
                    try
                    {
                       var classToWeave_L = new ClassToWeave(ref i);
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
            public ClassToWeave(ref byte i)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(ref byte i)
            {
                I = i;
                i = 14;
            }
        }
    }
}