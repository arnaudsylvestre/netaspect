using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Instance
{
    public class OnFinallyPropertyInstanceParameterWithRealTypeReferencedTest :
        NetAspectTest<OnFinallyPropertyInstanceParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method OnFinallyPropertyGetMethod of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Instance;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertyGetMethod(ref ClassToWeave instance)
            {
                Instance = instance;
            }
        }
    }
}