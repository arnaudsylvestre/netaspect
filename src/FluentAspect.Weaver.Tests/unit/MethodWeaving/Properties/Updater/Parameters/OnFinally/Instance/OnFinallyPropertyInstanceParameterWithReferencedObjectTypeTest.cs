using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Instance
{
    public class OnFinallyPropertyInstanceParameterWithReferencedObjectTypeTest :
        NetAspectTest<OnFinallyPropertyInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'instance' in the method OnFinallyPropertySetMethod of the type '{0}'", typeof(MyAspect)));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static object Instance;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertySetMethod(ref object instance)
            {
                Instance = instance;
            }
        }
    }
}