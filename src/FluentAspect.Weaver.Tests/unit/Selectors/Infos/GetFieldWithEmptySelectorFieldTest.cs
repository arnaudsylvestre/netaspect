using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.Selectors.Infos
{
    public class GetFieldWithEmptySelectorFieldTest :
        NetAspectTest<GetFieldWithEmptySelectorFieldTest.ClassToWeave>
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
                        "The selector SelectField of the aspect {0} must have the following parameters : field",
                        typeof(MyAspect).FullName, typeof(FieldInfo).FullName)
                });
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

            public static bool SelectField()
            {
                return false;
            }
        }
    }
}