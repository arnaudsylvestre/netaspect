using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.CallerParameterName
{
    public class BeforeCallUpdateFieldCallerParameterNameReferencedParameterWithRealTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCallerParameterNameReferencedParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    int val = 12;
                    classToWeave_L.Weaved(ref val);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved(ref int param1)
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}