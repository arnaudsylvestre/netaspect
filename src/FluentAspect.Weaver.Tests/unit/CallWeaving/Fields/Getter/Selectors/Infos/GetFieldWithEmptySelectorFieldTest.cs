using System;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Selectors.Infos
{
    public class GetFieldWithEmptySelectorFieldTest :
        NetAspectTest<GetFieldWithEmptySelectorFieldTest.ClassToWeave>
    {

        public class ClassToWeave
        {
            public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassToWeave caller)
            {
                Caller = caller;
            }

            public static bool SelectField()
            {
                return false;
            }
        }
    }
}