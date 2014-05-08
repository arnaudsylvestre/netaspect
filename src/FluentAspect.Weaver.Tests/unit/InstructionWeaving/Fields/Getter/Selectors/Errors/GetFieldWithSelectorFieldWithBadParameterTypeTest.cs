using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Selectors.Errors
{
    public class GetFieldWithSelectorFieldWithBadParameterTypeTest :
        NetAspectTest<GetFieldWithSelectorFieldWithBadParameterTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The parameter fieldName in the method SelectField of the aspect {0} is expected to be System.String",
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

            public static bool SelectField(int fieldName)
            {
                return true;
            }
        }
    }
}