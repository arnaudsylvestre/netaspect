using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Exceptions
{
    public class OnExceptionConstructorExceptionParameterWithRealTypeTest :
        NetAspectTest<OnExceptionConstructorExceptionParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Exception);
                    try
                    {
                        var classToWeave_L = new ClassToWeave();
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual("Hello", MyAspect.Exception.Message);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
                throw new Exception("Hello");
            }
        }

        public class MyAspect : Attribute
        {
            public static Exception Exception;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(Exception exception)
            {
                Exception = exception;
            }
        }
    }
}