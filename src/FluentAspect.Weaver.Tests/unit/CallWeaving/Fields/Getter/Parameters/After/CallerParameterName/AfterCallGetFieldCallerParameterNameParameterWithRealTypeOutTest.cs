using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.CallerParameterName
{
    public class AfterCallGetFieldCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<AfterCallGetFieldCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to out the parameter 'callerParam1' in the method AfterGetField of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved(int param1)
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterGetField(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}