using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Return
{
    public class CheckWeaveWithReturnVoidTest : NetAspectTest<CheckWeaveWithReturnVoidTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    new ClassToWeave().Weaved = "12";
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
              set { }
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