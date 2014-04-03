using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Property
{
    public class OnExceptionPropertySetterPropertyParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertySetterPropertyParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.MyProperty = "";
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual("MyProperty", MyAspect.Property.Name);
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
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySet(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}