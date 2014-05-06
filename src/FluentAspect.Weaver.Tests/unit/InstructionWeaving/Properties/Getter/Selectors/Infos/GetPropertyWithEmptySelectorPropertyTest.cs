using System;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Selectors.Infos
{
    public class GetPropertyWithEmptySelectorPropertyTest :
        NetAspectTest<GetPropertyWithEmptySelectorPropertyTest.ClassToWeave>
    {

        public class ClassToWeave
        {
            public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(ClassToWeave caller)
            {
                Caller = caller;
            }

            public static bool SelectProperty()
            {
                return false;
            }
        }
    }
}