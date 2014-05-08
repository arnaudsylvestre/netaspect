using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Selectors.Errors
{
    public class GetPropertyWithSelectorPropertyWithReferencedParameterTypeTest :
        NetAspectTest<GetPropertyWithSelectorPropertyWithReferencedParameterTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "The parameter propertyName in the method SelectProperty of the aspect {0} is expected to be System.String",
                        typeof(MyAspect).FullName));
        }

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

            public static bool SelectProperty(ref string propertyName)
            {
                return true;
            }
        }
    }
}