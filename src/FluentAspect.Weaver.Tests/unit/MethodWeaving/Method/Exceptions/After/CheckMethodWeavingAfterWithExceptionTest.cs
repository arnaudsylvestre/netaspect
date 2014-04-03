using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Exceptions.After
{
    public class CheckMethodWeavingAfterWithExceptionTest :
        NetAspectTest<CheckMethodWeavingAfterWithExceptionTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved(classToWeave_L);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.IsNull(MyAspect.Method);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave Weaved(ClassToWeave toWeave)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void After(MethodInfo method)
            {
                Method = method;
            }
        }
    }
}