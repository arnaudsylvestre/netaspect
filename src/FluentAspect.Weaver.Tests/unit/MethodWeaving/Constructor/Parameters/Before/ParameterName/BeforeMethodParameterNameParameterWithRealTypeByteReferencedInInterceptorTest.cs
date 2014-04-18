using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.ParameterName
{
    public class BeforeConstructorParameterNameParameterWithRealTypeByteReferencedInInterceptorTest :
        NetAspectTest<BeforeConstructorParameterNameParameterWithRealTypeByteReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    byte i = 12; 
                   var classToWeave_L = new ClassToWeave(i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(byte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static int I;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(ref byte i)
            {
                I = i;
            }
        }
    }
}