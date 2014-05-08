using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Constructor
{
    public class AfterConstructorConstructorInfoParameterWithRealTypeOutTest :
        NetAspectTest<AfterConstructorConstructorInfoParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("impossible to ref/out the parameter 'constructor' in the method AfterConstructor of the type '{0}'",
                                  typeof (MyAspect).FullName));
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

            public void AfterConstructor(out MethodBase constructor)
            {
                constructor = null;
            }
        }
    }
}