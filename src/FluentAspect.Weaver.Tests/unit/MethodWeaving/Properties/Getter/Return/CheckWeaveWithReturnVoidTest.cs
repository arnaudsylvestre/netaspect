using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Return
{
    public class CheckWeaveWithReturnVoidTest : NetAspectTest<CheckWeaveWithReturnVoidTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var value = new ClassToWeave().Weaved;
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
				get {return "12";}
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