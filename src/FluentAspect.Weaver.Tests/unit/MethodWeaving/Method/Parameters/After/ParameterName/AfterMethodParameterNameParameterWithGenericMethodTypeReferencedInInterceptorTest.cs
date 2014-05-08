using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
    public class AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest :
        NetAspectTest<AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("Impossible to ref a generic parameter"));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved<T>(T i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static object I;
            public bool NetAspectAttribute = true;

            public void After(ref object i)
            {
                I = i;
            }
        }
    }
}