using System;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.Caller
{
    public class GetFieldWithSelectorFieldNotStaticTest :
        NetAspectTest<GetFieldWithSelectorFieldNotStaticTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
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