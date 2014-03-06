using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Value
{
    public class OnExceptionPropertySetterValueParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertySetterValueParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Value);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.MyProperty = "Value";
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual("Value", MyAspect.Value);
                    }
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { throw new System.Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static string Value;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySet(string value)
            {
                Value = value;
            }
        }
    }
}