using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Property
{
    public class OnFinallyPropertyGetterPropertyParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertyGetterPropertyParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    string property = classToWeave_L.MyProperty;
                    Assert.AreEqual("MyProperty", MyAspect.Property.Name);
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                get { return ""; }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertyGet(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}