using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Errors
{
    public class AfterConstructorParameterNameSameAsKeywordTest :
        NetAspectTest<AfterConstructorParameterNameSameAsKeywordTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler =>
                {
                    errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message = (string.Format("The parameter instance is already declared"))});
                    errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                        string.Format(
                            "the instance parameter in the method AfterConstructor of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}", typeof(MyAspect), typeof(ClassToWeave))
                });
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(int instance)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructor(int instance)
            {
            }
        }
    }
}