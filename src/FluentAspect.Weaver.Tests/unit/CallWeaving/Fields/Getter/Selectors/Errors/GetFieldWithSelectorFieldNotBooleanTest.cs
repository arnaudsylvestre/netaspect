using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Selectors.Errors
{
    public class GetFieldWithSelectorFieldNotBooleanTest :
        NetAspectTest<GetFieldWithSelectorFieldNotBooleanTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The selector SelectField in the aspect {0} must return boolean value",
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

            public static int SelectField(string fieldName)
            {
                return 1;
            }
        }
    }
}