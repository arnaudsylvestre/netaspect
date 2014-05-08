using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Method
{
    public class AfterMethodMethodInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterMethodMethodInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format("impossible to ref/out the parameter 'method' in the method After of the type '{0}'",
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
           public static MethodBase Method;
            public bool NetAspectAttribute = true;

            public void After(ref MethodBase method)
            {
                Method = method;
            }
        }
    }
}