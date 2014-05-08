using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Selectors.Errors
{
    public class GetFieldWithSelectorFieldNotStaticTest :
        NetAspectTest<GetFieldWithSelectorFieldNotStaticTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The selector SelectField in the aspect {0} must be static",
                        typeof(MyAspect).FullName));
        }

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

            public bool SelectField(string fieldName)
            {
                return fieldName == "Field";
            }
        }
    }
}