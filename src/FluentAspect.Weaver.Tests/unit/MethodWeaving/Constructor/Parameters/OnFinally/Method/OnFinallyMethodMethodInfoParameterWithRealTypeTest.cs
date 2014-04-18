using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally.Method
{
    public class OnFinallyConstructorMethodBaseParameterWithRealTypeTest :
        NetAspectTest<OnFinallyConstructorMethodBaseParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodBase);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.MethodBase);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodBase MethodBase;
            public bool NetAspectAttribute = true;

            public void OnFinallyConstructor(MethodBase method)
            {
                MethodBase = method;
            }
        }
    }
}