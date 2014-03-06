using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeSingleReferencedInInterceptorTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSingleReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(3F);
                    Assert.AreEqual(3F, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(Single i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Single I;
            public bool NetAspectAttribute = true;

            public void After(ref Single i)
            {
                I = i;
            }
        }
    }
}