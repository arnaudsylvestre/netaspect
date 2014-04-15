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
                        classToWeave_L.Weaved(classToWeave_L);
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
            [MyAspect]
            public ClassToWeave Weaved(ClassToWeave toWeave)
            {
                if (toWeave != null)
                    throw new Exception();
                return toWeave;
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo Property;
            public static PropertyInfo FinallyProperty;
            public bool NetAspectAttribute = true;

            public void OnException(PropertyInfo property)
            {
                Property = property;
            }

            public void OnFinally(PropertyInfo property)
            {
                FinallyProperty = property;
            }
        }
    }
}