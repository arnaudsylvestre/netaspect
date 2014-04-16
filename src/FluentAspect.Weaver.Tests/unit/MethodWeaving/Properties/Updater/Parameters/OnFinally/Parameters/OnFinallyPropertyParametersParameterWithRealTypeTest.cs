using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Parameters
{
    public class OnFinallyPropertyParametersParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertyParametersParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Parameters);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved = (12);
                    Assert.AreEqual(new object[] {12}, MyAspect.Parameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public int Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertySetMethod(object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}