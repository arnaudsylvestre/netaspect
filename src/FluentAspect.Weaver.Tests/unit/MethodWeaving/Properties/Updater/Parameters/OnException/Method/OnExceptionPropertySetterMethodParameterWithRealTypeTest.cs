using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Method
{
    public class OnExceptionPropertySetterMethodParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertySetterMethodParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.MyProperty = "";
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual("set_MyProperty", MyAspect.Method.Name);
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
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySet(MethodInfo method)
            {
                Method = method;
            }
        }
    }
}