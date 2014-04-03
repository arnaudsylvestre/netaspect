using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Errors
{
    public class AfterMethodParameterNameSameAsKeywordTest :
        NetAspectTest<AfterMethodParameterNameSameAsKeywordTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler =>
                {
                    errorHandler.Errors.Add(string.Format("The parameter instance is already declared"));
                    errorHandler.Errors.Add(
                        string.Format(
                            "the instance parameter in the method After of the type '{0}", typeof(MyAspect)));
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved(int instance)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void After(int instance)
            {
            }
        }
    }
}