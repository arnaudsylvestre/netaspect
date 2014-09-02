using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Parameters
{
    public class OnExceptionMethodParametersParameterWithRealTypeTest :
        NetAspectTest<OnExceptionMethodParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Parameters);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved(12);
                        Assert.Fail();
                    }
                    catch
                    {
                    }
                    Assert.AreEqual(new object[] {12}, MyAspect.Parameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(int i)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void OnException(object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}