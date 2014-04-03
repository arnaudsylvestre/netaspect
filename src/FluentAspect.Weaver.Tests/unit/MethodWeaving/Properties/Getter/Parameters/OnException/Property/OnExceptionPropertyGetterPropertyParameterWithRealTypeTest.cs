using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Property
{
    public class OnExceptionPropertyGetterPropertyParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertyGetterPropertyParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        string property = classToWeave_L.MyProperty;
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
                get { throw new System.Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGet(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}