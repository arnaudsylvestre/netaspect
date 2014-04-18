using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeShortReferencedInBothTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeShortReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    short i = 12; 
                   var classToWeave_L = new ClassToWeave(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(8, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref short i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static short I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref short i)
            {
                I = i;
                i = 8;
            }
        }
    }
}