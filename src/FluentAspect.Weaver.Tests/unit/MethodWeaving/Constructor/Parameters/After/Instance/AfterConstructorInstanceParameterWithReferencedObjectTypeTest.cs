using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Instance
{
    public class AfterConstructorInstanceParameterWithReferencedObjectTypeTest :
        NetAspectTest<AfterConstructorInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method AfterConstructor of the type '{0}'", typeof(MyAspect)));
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

            public void AfterConstructor(ref object instance)
            {
                Instance = instance;
            }
        }
    }
}