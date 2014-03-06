using System;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.CalledParameterName
{
    public class AfterCallEventCalledParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterCallEventCalledParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.MyAspect).FullName,
                        typeof (AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public event Action Event;

            public void Weaved(int param1)
            {
                Event();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterRaiseEvent(string calledParam1)
            {
            }
        }
    }
}