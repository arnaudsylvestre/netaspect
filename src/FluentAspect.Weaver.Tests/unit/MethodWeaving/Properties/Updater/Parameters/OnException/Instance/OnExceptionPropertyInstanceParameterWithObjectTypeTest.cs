using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Instance
{
    public class OnExceptionPropertyInstanceParameterWithObjectTypeTest :
        NetAspectTest<OnExceptionPropertyInstanceParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved = "12";
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(classToWeave_L, MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
               set { throw new Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static object Instance;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySetMethod(object instance)
            {
                Instance = instance;
            }
        }
    }
}