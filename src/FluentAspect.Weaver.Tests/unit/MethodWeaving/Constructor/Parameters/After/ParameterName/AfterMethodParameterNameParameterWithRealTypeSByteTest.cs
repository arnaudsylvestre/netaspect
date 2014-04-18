using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeSByteTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeSByteTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(sbyte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static sbyte I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(sbyte i)
            {
                I = i;
            }
        }
    }
}