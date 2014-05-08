using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Instance
{
    public class AfterConstructorInstanceParameterWithBadTypeTest :
        NetAspectTest<AfterConstructorInstanceParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the instance parameter in the method AfterConstructor of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (MyAspect).FullName, typeof (ClassToWeave).FullName));
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
            public bool NetAspectAttribute = true;

            public void AfterConstructor(int instance)
            {
            }
        }
    }
}