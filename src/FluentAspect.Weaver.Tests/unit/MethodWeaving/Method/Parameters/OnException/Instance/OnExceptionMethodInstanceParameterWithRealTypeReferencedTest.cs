using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Instance
{
    public class OnExceptionMethodInstanceParameterWithRealTypeReferencedTest :
        NetAspectTest<OnExceptionMethodInstanceParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method OnException of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnException(ref ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}