using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Attributes.Static
{
    public class WeaveStaticPropertyTest : NetAspectTest<WeaveStaticPropertyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var value = ClassToWeave.Weaved;
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void Before(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}