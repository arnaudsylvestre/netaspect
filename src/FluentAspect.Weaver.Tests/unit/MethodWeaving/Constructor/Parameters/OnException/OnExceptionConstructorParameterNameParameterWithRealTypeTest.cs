using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
{
    public class OnExceptionConstructorParameterNameParameterWithRealTypeTest :
        NetAspectTest<OnExceptionConstructorParameterNameParameterWithRealTypeTest.ClassToWeave>
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
                    catch (Exception)
                    {
                        Assert.AreEqual(12, MyAspect.I);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(int i)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void OnException(int i)
            {
                I = i;
            }
        }
    }
}