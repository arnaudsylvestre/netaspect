using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Selectors.Errors
{
    public class UpdateFieldWithSelectorFieldNotStaticTest :
        NetAspectTest<UpdateFieldWithSelectorFieldNotStaticTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "The selector SelectField in the aspect {0} must be static",
                        typeof(MyAspect).FullName)
                });
        }

        public class ClassToWeave
        {
            public string Field;

            public void Weaved()
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(ClassToWeave caller)
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