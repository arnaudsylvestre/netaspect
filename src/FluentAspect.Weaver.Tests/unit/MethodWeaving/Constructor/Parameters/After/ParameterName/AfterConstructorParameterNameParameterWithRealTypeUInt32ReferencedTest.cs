using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeUInt32ReferencedTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeUInt32ReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    UInt32 i_L = 12; 
                   var classToWeave_L = new ClassToWeave(ref i_L);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref UInt32 i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static UInt32 I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(UInt32 i)
            {
                I = i;
            }
        }
    }
}