using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Parameters
{
    public class OnExceptionPropertyParametersParameterWithRealTypeTest :
        NetAspectTest<OnExceptionPropertyParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Parameters);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved=12;
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
            public int Weaved
            {
               set { throw new Exception(); }
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySetMethod(object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}