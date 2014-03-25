using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Updater.Parameters.After.CallerParameterName
{
    public class AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallUpdateFieldCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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
            [MyAspect] public string Field;

            public void Weaved(int param1)
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(ref int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}