using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.CallerParameterName
{
    public class BeforeCallUpdateFieldCallerParameterNameParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
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
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(object callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}