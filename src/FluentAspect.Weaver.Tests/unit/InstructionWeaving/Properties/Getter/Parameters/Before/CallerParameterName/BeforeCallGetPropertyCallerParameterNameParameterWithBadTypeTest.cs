using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.CallerParameterName
{
    public class BeforeCallGetPropertyCallerParameterNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallGetPropertyCallerParameterNameParameterWithBadTypeTest.ClassToWeave>
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
                        "the callerParam1 parameter in the method BeforeGetProperty of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32",
                        typeof(MyAspect).FullName,
                        typeof(ClassToWeave).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved(int param1)
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(string callerParam1)
            {
            }
        }
    }
}