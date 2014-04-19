using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Exceptions.OnException
{
    public class CheckPropertyWeavingOnExceptionWithExceptionAndFinallyTest :
        NetAspectTest<CheckPropertyWeavingOnExceptionWithExceptionAndFinallyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Property);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.toWeave = classToWeave_L;
                        var value = classToWeave_L.Weaved;
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.AreEqual("Weaved", MyAspect.Property.Name);
                        Assert.AreEqual("Weaved", MyAspect.FinallyProperty.Name);
                    }
                };
        }

        public class ClassToWeave
        {
            public ClassToWeave toWeave;

            [MyAspect]
            public ClassToWeave Weaved
            {
                get
                {
                    if (toWeave != null)
                        throw new Exception();
                    return toWeave;
                }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public static PropertyInfo FinallyProperty;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGetMethod(PropertyInfo property)
            {
                Property = property;
            }

            public void OnFinallyPropertyGetMethod(PropertyInfo property)
            {
                FinallyProperty = property;
            }
        }
    }
}