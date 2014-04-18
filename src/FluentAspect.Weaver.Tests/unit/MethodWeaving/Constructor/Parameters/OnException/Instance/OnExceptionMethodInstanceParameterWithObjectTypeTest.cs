using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Instance
{
    public class OnExceptionConstructorInstanceParameterWithObjectTypeTest :
        NetAspectTest<OnExceptionConstructorInstanceParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Instance);
                    try
                    {
                       var classToWeave_L = new ClassToWeave();
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.NotNull(MyAspect.Instance);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static object Instance;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(object instance)
            {
                Instance = instance;
            }
        }
    }
}