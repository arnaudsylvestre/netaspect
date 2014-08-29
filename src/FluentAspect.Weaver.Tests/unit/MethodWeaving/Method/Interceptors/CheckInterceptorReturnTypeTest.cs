using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Interceptors
{
    public class CheckInterceptorReturnTypeTest : NetAspectTest<CheckInterceptorReturnTypeTest.ClassToWeave>
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
                        "The Before interceptor in the {0} aspect must be void",
                        typeof(MyAspect).FullName)
                });
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

            public bool Before(ClassToWeave instance)
            {
                Instance = instance;
                return false;
            }
        }
    }
}