using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeUshortReferencedTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeUshortReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    ushort i_L = 12; 
                   var classToWeave_L = new ClassToWeave(ref i_L);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref ushort i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static ushort I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ushort i)
            {
                I = i;
            }
        }
    }
}