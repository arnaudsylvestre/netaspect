using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithGenericMethodTypeTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithGenericMethodTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.I);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.I);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved<T>(T i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static object I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(object i)
            {
                I = i;
            }
        }
    }
}