using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Return
{
    public class CheckWeaveWithReturnIntTest : NetAspectTest<CheckWeaveWithReturnIntTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    int res = new ClassToWeave().Weaved();
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                    Assert.AreEqual(12, res);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public int Weaved()
            {
                return 12;
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void BeforePropertySetMethod(PropertyInfo Property)
            {
                Property = Property;
            }
        }
    }
}