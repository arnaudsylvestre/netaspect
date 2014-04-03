using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
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
                    catch (Exception)
                    {
                        Assert.AreEqual("Message", MyAspect.Exception.Message);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
                throw new Exception("Message");
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