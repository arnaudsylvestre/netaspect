using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Exceptions
{
    public class OnExceptionMethodExceptionParameterWithRealTypeTest :
        NetAspectTest<OnExceptionMethodExceptionParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Exception);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved();
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
            public void Weaved()
            {
                throw new Exception("Hello");
            }
        }

        public class MyAspect : Attribute
        {
            public static Exception Exception;
            public bool NetAspectAttribute = true;

            public void OnException(Exception exception)
            {
                Exception = exception;
            }
        }
    }
}