using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Property
{
    public class OnFinallyPropertySetterPropertyParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertySetterPropertyParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.MyProperty = "";
                    Assert.AreEqual("MyProperty", MyAspect.Property.Name);
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertySet(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}