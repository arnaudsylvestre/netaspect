using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeInt64ReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeInt64ReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    Int64 i = 12;
                    var classToWeave_L = new ClassToWeave(i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(Int64 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Int64 I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref Int64 i)
            {
                I = i;
            }
        }
    }
}