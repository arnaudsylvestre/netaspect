using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.ParameterName
{
    public class BeforeConstructorParameterNameParameterWithRealTypeByteReferencedTest :
        NetAspectTest<BeforeConstructorParameterNameParameterWithRealTypeByteReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    byte i = 12; 
                   var classToWeave_L = new ClassToWeave(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref byte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(byte i)
            {
                I = i;
            }
        }
    }
}