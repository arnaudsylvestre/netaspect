using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithBadTypeTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the i parameter in the method AfterConstructor of the type '{0}' is declared with the type 'System.String' but it is expected to be {1} because of the type of this parameter in the method .ctor of the type {2}",
                        typeof (MyAspect).FullName, typeof (int), typeof (ClassToWeave)));
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

            public void AfterConstructor(string i)
            {
            }
        }
    }
}