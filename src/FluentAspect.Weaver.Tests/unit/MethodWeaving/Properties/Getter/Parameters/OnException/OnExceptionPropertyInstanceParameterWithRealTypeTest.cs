using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Instance
{
    public class OnExceptionPropertyInstanceParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertyInstanceParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        var value = classToWeave_L.Weaved;
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
                get { throw new Exception(); }
                
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertyGetMethod(ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}