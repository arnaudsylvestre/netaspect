using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.ParameterName
{
    public class OnExceptionConstructorParameterNameParameterWithRealTypeByteReferencedInInterceptorTest :
        NetAspectTest<OnExceptionConstructorParameterNameParameterWithRealTypeByteReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    try
                    {
                    var classToWeave_L = new ClassToWeave(12);

                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(byte i)
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
            }
        }
    }
}