using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.CalledParameterName
{
    public class BeforeCallEventCalledParameterNameParameterWithRealTypeReferencedTest : NetAspectTest<BeforeCallEventCalledParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.AreEqual(0, MyAspect.ParameterName);
                var classToWeave_L = new ClassToWeave();
                classToWeave_L.Weaved(12);
                Assert.AreEqual(12, MyAspect.ParameterName);
            };
        }

        public class ClassToWeave
        {

            [MyAspect]
            public event Action Event;

            public void Weaved(int param1)
            {
                Event();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static int ParameterName;

            public void BeforeRaiseEvent(ref int calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }


}