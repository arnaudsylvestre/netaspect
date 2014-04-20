using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Constructor
{
    public class OnExceptionConstructorMethodBaseParameterWithRealTypeTest :
        NetAspectTest<OnExceptionConstructorMethodBaseParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodBase);
                    
                    try
                    {
                       var classToWeave_L = new ClassToWeave();
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.NotNull(MyAspect.MethodBase);
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
            public static MethodBase MethodBase;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(MethodBase constructor)
            {
               MethodBase = constructor;
            }
        }
    }
}