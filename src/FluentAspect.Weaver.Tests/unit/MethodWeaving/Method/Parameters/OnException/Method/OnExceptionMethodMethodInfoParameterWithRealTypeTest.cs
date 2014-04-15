using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Method
{
    public class OnExceptionMethodMethodBaseParameterWithRealTypeTest :
        NetAspectTest<OnExceptionMethodMethodBaseParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodBase);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved();
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.MethodBase);
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
            public static MethodBase MethodBase;
            public bool NetAspectAttribute = true;

            public void OnException(MethodBase method)
            {
                MethodBase = method;
            }
        }
    }
}