using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Value
{
    public class OnExceptionPropertyValueParameterWithRealTypeByteReferencedInInterceptorTest :
        NetAspectTest<OnExceptionPropertyValueParameterWithRealTypeByteReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved = 12;
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
            public byte Weaved
            {
               set { throw new Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySetMethod(ref byte value)
            {
                I = value;
            }
        }
    }
}