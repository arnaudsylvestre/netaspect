using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.CallerParameterName
{
    public class AfterCallGetPropertyCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<AfterCallGetPropertyCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to out the parameter 'callerParam1' in the method AfterGetProperty of the type '{0}'",
                        typeof (MyAspect).FullName));
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

            public void AfterGetProperty(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}