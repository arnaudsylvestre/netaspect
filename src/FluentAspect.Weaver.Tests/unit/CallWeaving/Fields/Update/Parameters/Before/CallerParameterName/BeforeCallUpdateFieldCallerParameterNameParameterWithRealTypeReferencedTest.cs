using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.CallerParameterName
{
    public class BeforeCallUpdateFieldCallerParameterNameParameterWithRealTypeReferencedTest : NetAspectTest<BeforeCallUpdateFieldCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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
            public string Field;

            public void Weaved(int param1)
            {
                Field = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static int ParameterName;

            public void BeforeUpdateField(ref int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }


}