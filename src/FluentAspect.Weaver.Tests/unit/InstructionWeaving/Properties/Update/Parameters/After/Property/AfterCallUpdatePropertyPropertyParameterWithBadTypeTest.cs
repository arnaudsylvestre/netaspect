using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.Property
{
    public class AfterCallUpdatePropertyPropertyParameterWithBadTypeTest :
        NetAspectTest<AfterCallUpdatePropertyPropertyParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the property parameter in the method AfterSetProperty of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Reflection.PropertyInfo",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(string property)
            {
            }
        }
    }
}