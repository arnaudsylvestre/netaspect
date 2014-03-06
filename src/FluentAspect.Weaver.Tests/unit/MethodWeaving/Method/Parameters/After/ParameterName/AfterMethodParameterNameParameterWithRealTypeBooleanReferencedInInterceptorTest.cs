using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeBooleanReferencedInInterceptorTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeBooleanReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(false, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(true);
                    Assert.AreEqual(true, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(Boolean i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Boolean I;
            public bool NetAspectAttribute = true;

            public void After(ref Boolean i)
            {
                I = i;
            }
        }
    }
}