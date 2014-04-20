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
            public string Weaved()
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