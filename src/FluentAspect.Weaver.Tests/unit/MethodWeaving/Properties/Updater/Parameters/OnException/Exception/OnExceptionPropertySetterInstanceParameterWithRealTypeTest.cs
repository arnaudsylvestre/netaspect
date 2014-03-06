using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Exception
{
    public class OnExceptionPropertySetterInstanceParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Exception);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.MyProperty = "";
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual("Message", MyAspect.Exception.Message);
                    }
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { throw new System.Exception("Message"); }
            }
        }

        public class MyAspect : Attribute
        {
            public static System.Exception Exception;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySet(System.Exception exception)
            {
                Exception = exception;
            }
        }
    }
}