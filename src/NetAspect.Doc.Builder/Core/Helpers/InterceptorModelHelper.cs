using System;

namespace NetAspect.Doc.Builder.Model
{
    public static class InterceptorModelHelper
    {
        public static Event ExtractEvent(string interceptorName)
        {
            if (interceptorName.Contains("Before"))
                return Event.Before;
            if (interceptorName.Contains("After"))
                return Event.After;
            if (interceptorName.Contains("Exception"))
                return Event.OnException;
            if (interceptorName.Contains("Finally"))
                return Event.OnFinally;
            throw new NotSupportedException(interceptorName);
        }

        public static Kind ExtractKind(string interceptorName)
        {
            if (interceptorName.Contains("Call"))
                return Kind.Call;
            if (interceptorName.Contains("Parameter"))
                return Kind.Parameter;
            return Kind.Method;
        }
    }
}