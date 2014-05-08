using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.CallerParameterName
{
    public class BeforeCallUpdatePropertyCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<BeforeCallUpdatePropertyCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to out the parameter 'callerParam1' in the method BeforeSetProperty of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved(int param1)
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeSetProperty(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}