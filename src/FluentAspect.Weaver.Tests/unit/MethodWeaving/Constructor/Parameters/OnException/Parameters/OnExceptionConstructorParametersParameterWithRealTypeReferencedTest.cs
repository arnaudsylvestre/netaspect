using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Parameters
{
    public class OnExceptionConstructorParametersParameterWithRealTypeReferencedTest :
        NetAspectTest<OnExceptionConstructorParametersParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "impossible to ref/out the parameter 'parameters' in the method OnExceptionConstructor of the type '{0}'",
                        typeof (MyAspect).FullName)});
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static object[] Parameters;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(ref object[] parameters)
            {
                Parameters = parameters;
            }
        }
    }
}