using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeSByteReferencedInBothTest :
        NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSByteReferencedInBothTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    sbyte i = 12;
                    classToWeave_L.Weaved(ref i);
                    Assert.AreEqual(12, MyAspect.I);
                    Assert.AreEqual(15, i);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(ref sbyte i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static sbyte I;
            public bool NetAspectAttribute = true;

            public void After(ref sbyte i)
            {
                I = i;
                i = 15;
            }
        }
    }
}