using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.Before.Instance
{
    public class BeforePropertyInstanceParameterWithReferencedObjectTypeTest :
        NetAspectTest<BeforePropertyInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method BeforePropertyGetMethod of the type '{0}'", typeof(MyAspect)));
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
            public static object Instance;
            public bool NetAspectAttribute = true;

            public void BeforePropertyGetMethod(ref object instance)
            {
                Instance = instance;
            }
        }
    }
}