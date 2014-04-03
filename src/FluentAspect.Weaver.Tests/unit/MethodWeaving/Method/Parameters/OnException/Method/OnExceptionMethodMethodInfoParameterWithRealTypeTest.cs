using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Method
{
    public class OnExceptionMethodMethodInfoParameterWithRealTypeTest :
        NetAspectTest<OnExceptionMethodMethodInfoParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodInfo);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved();
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.MethodInfo);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved()
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo MethodInfo;
            public bool NetAspectAttribute = true;

            public void OnException(MethodInfo method)
            {
                MethodInfo = method;
            }
        }
    }
}