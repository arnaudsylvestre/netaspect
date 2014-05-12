using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.CalledParameterName
{
    public class BeforeCallMethodCalledParameterNameReferencedParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodCalledParameterNameReferencedParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    
                    classToWeave_L.Weaved();
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Method(ref int param1)
            {
                param1 = 15;
            }

            public void Weaved()
            {
                int val = 12;
                Method(ref val);
                Assert.AreEqual(15, val);
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}