using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Return
{
    public class CheckWeaveWithReturnClassTest : NetAspectTest<CheckWeaveWithReturnClassTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                   classToWeave_L.toWeave = classToWeave_L;
                    ClassToWeave res = classToWeave_L.Weaved;
                    Assert.AreEqual("Weaved", MyAspect.Property.Name);
                    Assert.AreEqual(classToWeave_L, res);
                };
        }

        public class ClassToWeave
        {
           public ClassToWeave toWeave;
            [MyAspect]
            public ClassToWeave Weaved
            {
               get { return toWeave; }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void BeforePropertyGetMethod(PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}