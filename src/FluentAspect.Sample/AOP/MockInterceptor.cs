using System;
using System.Reflection;

namespace FluentAspect.Sample.AOP
{
    public class MockInterceptorAttribute : Attribute
    {
        public static BeforeInfo before;
        public static AfterInfo after;
        public static ExceptionInfo exception;
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object instance, MethodInfo method, object[] parameters)
        {
            before = new BeforeInfo
                {
                    @this = instance,
                    methodInfo_P = method,
                    parameters = parameters
                };
        }

        public void After(object instance, MethodInfo method, object[] parameters, ref string result)
        {
            after = new AfterInfo
                {
                    @this = instance,
                    result = result,
                    methodInfo_P = method,
                    parameters = parameters
                };
        }

        public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
        {
            MockInterceptorAttribute.exception = new ExceptionInfo
                {
                    @this = instance,
                    methodInfo_P = method,
                    parameters = parameters,
                    Exception = exception,
                };
        }

        public class AfterInfo
        {
            public MethodInfo methodInfo_P;
            public object[] parameters;
            public object result;
            public object @this;
        }

        public class BeforeInfo
        {
            public MethodInfo methodInfo_P;
            public object[] parameters;
            public object @this;
        }

        public class ExceptionInfo
        {
            public Exception Exception;
            public MethodInfo methodInfo_P;
            public object[] parameters;
            public object @this;
        }
    }
}