using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Method
{
    public class OnFinallyMethodMethodBaseParameterWithRealTypeTest :
        NetAspectTest<OnFinallyMethodMethodBaseParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodBase);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.MethodBase);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodBase MethodBase;
            public bool NetAspectAttribute = true;

            public void OnFinally(MethodBase method)
            {
                MethodBase = method;
            }
        }
    }
}