using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Attributes.Static
{
    public class WeaveStaticPropertyTest : NetAspectTest<WeaveStaticPropertyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    ClassToWeave.Weaved = "12";
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static string Weaved
            {
               set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void BeforePropertySetMethod(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}