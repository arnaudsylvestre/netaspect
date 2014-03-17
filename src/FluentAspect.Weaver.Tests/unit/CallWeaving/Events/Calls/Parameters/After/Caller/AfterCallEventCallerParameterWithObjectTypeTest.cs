using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.Caller
{
    public class AfterCallEventCallerParameterWithObjectTypeTest :
        NetAspectTest<AfterCallEventCallerParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Event += () => { };
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public event Action Event;

            public void Weaved()
            {
                Event();
            }
        }

        public class MyAspect : Attribute
        {
            public static object Caller;
            public bool NetAspectAttribute = true;

            public void AfterRaiseEvent(object caller)
            {
                Caller = caller;
            }
        }
    }
}