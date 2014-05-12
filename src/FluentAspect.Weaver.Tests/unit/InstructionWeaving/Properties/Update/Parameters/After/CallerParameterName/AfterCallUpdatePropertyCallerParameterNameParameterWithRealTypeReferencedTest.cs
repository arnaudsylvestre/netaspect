using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.CallerParameterName
{
    public class AfterCallUpdatePropertyCallerParameterNameParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallUpdatePropertyCallerParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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
            [MyAspect] public string Property {get;set;}

            public void Weaved(int param1)
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(ref int callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}