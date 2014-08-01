using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Errors
{
    public class AfterConstructorParameterNameUnkownTest : NetAspectTest<AfterConstructorParameterNameUnkownTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message = (string.Format("The parameter 'unknown' is unknown. Expected one of : instance, constructor, parameters, i"))
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(int i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructor(string unknown)
            {
            }
        }
    }
}