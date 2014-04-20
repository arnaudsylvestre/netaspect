using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally.Constructor
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
                    Assert.AreEqual(".ctor", MyAspect.MethodBase.Name);
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

            public void OnFinallyConstructor(MethodBase constructor)
            {
                MethodBase = constructor;
            }
        }
    }
}