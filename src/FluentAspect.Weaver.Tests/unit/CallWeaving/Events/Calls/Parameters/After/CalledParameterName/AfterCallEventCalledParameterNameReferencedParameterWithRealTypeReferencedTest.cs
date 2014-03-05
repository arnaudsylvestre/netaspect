using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.CalledParameterName
{
    public class AfterCallEventCalledParameterNameReferencedParameterWithRealTypeReferencedTest : NetAspectTest<AfterCallEventCalledParameterNameReferencedParameterWithRealTypeReferencedTest.ClassToWeave>
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

            public void AfterRaiseEvent(ref int calledParam1)
            {
                ParameterName = calledParam1;
                calledParam1 = 25;
            }
        }
    }


}