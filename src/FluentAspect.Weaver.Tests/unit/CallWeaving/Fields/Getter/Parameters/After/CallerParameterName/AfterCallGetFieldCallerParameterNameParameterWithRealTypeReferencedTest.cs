using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.ParameterName
{
    public class AfterCallGetFieldCallerParameterNameParameterWithRealTypeReferencedTest : NetAspectTest<AfterCallGetFieldCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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

            public string Weaved(int param1)
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public static int ParameterName;

            public void AfterGetField(ref int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }


}