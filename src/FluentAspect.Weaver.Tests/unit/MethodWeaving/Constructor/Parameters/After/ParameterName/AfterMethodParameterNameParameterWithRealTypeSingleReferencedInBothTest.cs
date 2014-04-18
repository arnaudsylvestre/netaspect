using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeSingleReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeSingleReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    float f = 3F; 
                   var classToWeave_L = new ClassToWeave(ref f);
                    Assert.AreEqual(3F, MyAspect.I);
                    Assert.AreEqual(6F, f);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref Single i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Single I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref Single i)
            {
                I = i;
                i = 6;
            }
        }
    }
}