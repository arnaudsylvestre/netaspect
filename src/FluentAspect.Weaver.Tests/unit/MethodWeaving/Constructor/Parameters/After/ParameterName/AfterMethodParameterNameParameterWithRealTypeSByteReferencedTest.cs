using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithRealTypeSByteReferencedTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithRealTypeSByteReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    sbyte i = 12; 
                   var classToWeave_L = new ClassToWeave(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ref sbyte i)
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