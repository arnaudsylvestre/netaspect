using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.After.ColumnNumber
{
    public class AfterCallSubscribeEventColumnNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallSubscribeEventColumnNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ColumnNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(12, MyAspect.ColumnNumber);
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
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void AfterSubscribeEvent(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}