using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.FilePath
{
    public class AfterCallEventFilePathParameterWithRealTypeTest :
        NetAspectTest<AfterCallEventFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("", MyAspect.FilePath);
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
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void AfterRaiseEvent(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}