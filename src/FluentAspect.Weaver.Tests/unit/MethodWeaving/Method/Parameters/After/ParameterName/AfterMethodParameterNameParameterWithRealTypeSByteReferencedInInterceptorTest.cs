using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeSByteReferencedInInterceptorTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSByteReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(sbyte i)
            {

            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static sbyte I;

            public void After(ref sbyte i)
            {
                I = i;
            }
        }
    }
}