using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.Field
{
    public class AfterCallEventFieldParameterWithRealTypeTest :
        NetAspectTest<AfterCallEventFieldParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.Field);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("Field", MyAspect.Field.Name);
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
            public static FieldInfo Field;
            public bool NetAspectAttribute = true;

            public void AfterRaiseEvent(FieldInfo columnNumber)
            {
                Field = columnNumber;
            }
        }
    }
}