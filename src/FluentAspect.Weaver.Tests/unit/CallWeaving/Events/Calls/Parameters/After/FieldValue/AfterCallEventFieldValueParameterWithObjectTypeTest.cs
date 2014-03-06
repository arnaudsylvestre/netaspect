using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.FieldValue
{
    public class AfterCallEventFieldValueParameterWithObjectTypeTest :
        NetAspectTest<AfterCallEventFieldValueParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.FieldValue);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.FieldValue);
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
            public static object FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterRaiseEvent(object fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}