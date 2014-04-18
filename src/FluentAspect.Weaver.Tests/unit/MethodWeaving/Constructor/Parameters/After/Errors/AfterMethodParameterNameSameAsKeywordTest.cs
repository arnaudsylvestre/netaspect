using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Errors
{
    public class AfterConstructorParameterNameSameAsKeywordTest :
        NetAspectTest<AfterConstructorParameterNameSameAsKeywordTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler =>
                {
                    errorHandler.Errors.Add(string.Format("The parameter instance is already declared"));
                    errorHandler.Errors.Add(
                        string.Format(
                            "the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(MyAspect), typeof(ClassToWeave)));
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