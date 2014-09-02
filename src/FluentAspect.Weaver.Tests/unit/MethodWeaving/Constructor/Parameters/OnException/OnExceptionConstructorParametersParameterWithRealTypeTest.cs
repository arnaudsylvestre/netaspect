using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Parameters
{
    public class OnExceptionConstructorParametersParameterWithRealTypeTest :
        NetAspectTest<OnExceptionConstructorParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Parameters);
                    try
                    {
                       var classToWeave_L = new ClassToWeave(12);
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
            public ClassToWeave(int i)
            {
                throw new Exception();
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}