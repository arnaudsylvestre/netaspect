using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeSingleTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeSingleTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave(3F);
                    Assert.AreEqual(3F, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(Single i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static Single I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(Single i)
            {
                I = i;
            }
        }
    }
}