using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.CallerParameterName
{
    public class BeforeCallGetPropertyCallerParameterNameParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
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
            [MyAspect] public string Property {get; set; }

            public string Weaved(int param1)
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(object callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}