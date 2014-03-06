using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Method
{
    public class OnExceptionPropertyGetterMethodParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertyGetterMethodParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        string property = classToWeave_L.MyProperty;
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual("get_MyProperty", MyAspect.Method.Name);
                    }
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                get { throw new System.Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGet(MethodInfo method)
            {
                Method = method;
            }
        }
    }
}