using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Return
{
    public class CheckWeaveWithReturnGenericTest : NetAspectTest<CheckWeaveWithReturnGenericTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    ClassToWeave res = classToWeave_L.Weaved(classToWeave_L);
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                    Assert.AreEqual(classToWeave_L, res);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public T Weaved<T>(T toWeave)
            {
                return toWeave;
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