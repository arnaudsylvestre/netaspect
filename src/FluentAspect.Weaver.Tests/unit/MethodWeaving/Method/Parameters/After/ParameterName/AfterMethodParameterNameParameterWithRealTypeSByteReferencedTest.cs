using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithRealTypeSByteReferencedTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSByteReferencedTest.ClassToWeave>
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
            public bool NetAspectAttribute = true;

            public static sbyte I;

            public void After(sbyte i)
            {
                I = i;
            }
        }
    }
}