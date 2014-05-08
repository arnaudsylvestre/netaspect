using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
    public class BeforeMethodInstanceParameterWithRealTypeOutTest :
        NetAspectTest<BeforeMethodInstanceParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method Before of the type '{0}'",
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
            public bool NetAspectAttribute = true;

            public void Before(out ClassToWeave instance)
            {
                instance = null;
            }
        }
    }
}