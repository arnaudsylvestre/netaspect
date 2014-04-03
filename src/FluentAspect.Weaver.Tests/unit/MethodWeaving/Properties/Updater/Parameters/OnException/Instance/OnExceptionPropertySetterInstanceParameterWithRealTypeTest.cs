using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Instance
{
    public class OnExceptionPropertySetterInstanceParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.MyProperty = "";
                        Assert.Fail();
                    }
                    catch (System.Exception)
                    {
                        Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                    }
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { throw new System.Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySet(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}