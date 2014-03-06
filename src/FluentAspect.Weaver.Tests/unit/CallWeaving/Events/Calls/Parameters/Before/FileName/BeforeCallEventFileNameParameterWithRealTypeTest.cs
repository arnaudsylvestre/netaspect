using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.FileName
{
    public class BeforeCallEventFileNameParameterWithRealTypeTest :
        NetAspectTest<BeforeCallEventFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("", MyAspect.FileName);
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
            public static string FileName;
            public bool NetAspectAttribute = true;

            public void BeforeRaiseEvent(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}