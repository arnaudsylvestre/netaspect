using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.CalledParameterName
{
    public class AfterCallEventCalledParameterNameReferencedParameterWithRealTypeTest : NetAspectTest<AfterCallEventCalledParameterNameReferencedParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.AreEqual(0, MyAspect.ParameterName);
                var classToWeave_L = new ClassToWeave();
                int val = 12;
                classToWeave_L.Weaved(ref val);
                Assert.AreEqual(25, val);
                Assert.AreEqual(12, MyAspect.ParameterName);
            };
        }

        public class ClassToWeave
        {

            [MyAspect]
            public event Action Event;

            public void Weaved(ref int param1)
            {
                Event();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static int ParameterName;

            public void AfterRaiseEvent(int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }


}