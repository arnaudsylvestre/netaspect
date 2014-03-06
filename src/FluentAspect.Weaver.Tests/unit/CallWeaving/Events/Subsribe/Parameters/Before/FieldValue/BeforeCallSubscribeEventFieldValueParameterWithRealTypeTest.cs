using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before.FieldValue
{
    public class BeforeCallSubscribeEventFieldValueParameterWithRealTypeTest :
        NetAspectTest<BeforeCallSubscribeEventFieldValueParameterWithRealTypeTest.ClassToWeave>
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
                Event += () => { };
            }
        }

        public class MyAspect : Attribute
        {
            public static string FieldValue;
            public bool NetAspectAttribute = true;

            public void BeforeRaiseEvent(string fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}