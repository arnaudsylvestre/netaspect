using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.Caller
{
    public class BeforeCallMethodCallerParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodCallerParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(ClassToWeave caller)
            {
                Caller = caller;
            }
        }
    }
}