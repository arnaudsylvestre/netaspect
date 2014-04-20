using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.Constructor
{
    public class BeforeConstructorConstructorParameterWithRealTypeReferencedTest :
        NetAspectTest<BeforeConstructorConstructorParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'constructor' in the method BeforeConstructor of the type '{0}'",
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
            public static MethodBase Method;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(ref MethodBase constructor)
            {
                Method = constructor;
            }
        }
    }
}