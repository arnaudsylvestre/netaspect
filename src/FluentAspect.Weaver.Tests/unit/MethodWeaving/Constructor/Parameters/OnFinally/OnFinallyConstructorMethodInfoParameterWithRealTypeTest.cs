using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally
{
    public class OnFinallyConstructorMethodInfoParameterWithRealTypeTest :
        NetAspectTest<OnFinallyConstructorMethodInfoParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.MethodInfo);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual(".ctor", MyAspect.MethodInfo.Name);
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
            public static MethodBase MethodInfo;
            public bool NetAspectAttribute = true;

            public void OnFinally(MethodBase method)
            {
                MethodInfo = method;
            }
        }
    }
}