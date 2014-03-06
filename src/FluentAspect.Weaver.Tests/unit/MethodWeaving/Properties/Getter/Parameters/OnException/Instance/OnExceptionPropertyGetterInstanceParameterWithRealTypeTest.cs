using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Instance
{
    public class OnExceptionPropertyGetterInstanceParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertyGetterInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        string property = classToWeave_L.MyProperty;
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
                get { throw new System.Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGet(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}