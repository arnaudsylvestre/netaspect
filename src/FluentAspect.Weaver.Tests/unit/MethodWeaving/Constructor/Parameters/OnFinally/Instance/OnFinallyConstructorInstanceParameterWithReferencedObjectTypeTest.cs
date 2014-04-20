using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnFinally.Instance
{
    public class OnFinallyConstructorInstanceParameterWithReferencedObjectTypeTest :
        NetAspectTest<OnFinallyConstructorInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method OnFinallyConstructor of the type '{0}'", typeof(MyAspect)));
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
            public static object Instance;
            public bool NetAspectAttribute = true;

            public void OnFinallyConstructor(ref object instance)
            {
                Instance = instance;
            }
        }
    }
}