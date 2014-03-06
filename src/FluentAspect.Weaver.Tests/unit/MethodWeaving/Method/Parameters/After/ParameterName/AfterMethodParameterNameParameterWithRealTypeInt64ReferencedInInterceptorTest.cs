using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeInt64ReferencedInInterceptorTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeInt64ReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    Int64 i = 12;
                    classToWeave_L.Weaved(i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(Int64 i)
            {

            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static Int64 I;

            public void After(ref Int64 i)
            {
                I = i;
            }
        }
    }
}