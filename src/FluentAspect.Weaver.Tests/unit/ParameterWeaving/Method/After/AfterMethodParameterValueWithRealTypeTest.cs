using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.ParameterWeaving.Method.After
{
    public class AfterMethodParameterValueWithRealTypeTest :
        NetAspectTest<AfterMethodParameterValueWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, Positive.Value);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, Positive.Value);
                };
        }

        public class ClassToWeave
        {
            public void Weaved([Positive] int mustBeGreaterThanZero)
            {
            }
        }

        public class Positive : Attribute
        {
            public static int Value;
            public bool NetAspectAttribute = true;

            public void BeforeParameter(int value)
            {
                Value = value;
            }
        }
    }
}