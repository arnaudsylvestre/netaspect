using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
{
    public class OnExceptionConstructorMethodInfoParameterWithRealTypeTest :
        NetAspectTest<OnExceptionConstructorMethodInfoParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodInfo);
                    try
                    {
                        var classToWeave_L = new ClassToWeave();
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.AreEqual(".ctor", MyAspect.MethodInfo.Name);
                    }
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
            public static MethodBase MethodInfo;
            public bool NetAspectAttribute = true;

            public void OnException(MethodBase method)
            {
                MethodInfo = method;
            }
        }
    }
}