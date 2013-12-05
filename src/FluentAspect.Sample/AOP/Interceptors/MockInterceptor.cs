using System;
using System.Reflection;
using FluentAspect.Core.Core;

namespace FluentAspect.Sample
{
    public class MockInterceptor : IInterceptor
    {
        public static BeforeInfo before;
        public static AfterInfo after;
        public static ExceptionInfo exception;

        public void Before(object thisObject, MethodInfo methodInfo_P, object[] parameters)
        {
            before = new BeforeInfo
                {
                    @this = thisObject,
                    methodInfo_P = methodInfo_P,
                    parameters = parameters
                };
        }

        public void After(object thisObject, MethodInfo methodInfo_P, object[] parameters, ref object result_P)
        {
            after = new AfterInfo
                {
                    @this = thisObject,
                    result = result_P,
                    methodInfo_P = methodInfo_P,
                    parameters = parameters
                };
        }

        public void OnException(object thisObject, MethodInfo methodInfo_P, object[] parameters, Exception e)
        {
            exception = new ExceptionInfo
                {
                    @this = thisObject,
                    methodInfo_P = methodInfo_P,
                    parameters = parameters,
                    Exception = e,
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