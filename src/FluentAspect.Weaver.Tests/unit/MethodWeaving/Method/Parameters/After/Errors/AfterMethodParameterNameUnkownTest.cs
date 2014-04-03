using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Errors
{
    public class AfterMethodParameterNameUnkownTest : NetAspectTest<AfterMethodParameterNameUnkownTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("The parameter 'unknown' is unknown"));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(int i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void After(string unknown)
            {
            }
        }
    }
}