using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.After.LineNumber
{
    public class AfterCallSubscribeEventLineNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallSubscribeEventLineNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.LineNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(12, MyAspect.LineNumber);
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
            public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void AfterSubscribeEvent(int lineNumber)
            {
                LineNumber = lineNumber;
            }
        }
    }
}