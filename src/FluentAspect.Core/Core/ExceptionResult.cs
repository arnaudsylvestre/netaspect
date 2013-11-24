using System;

namespace FluentAspect.Core.Core
{
    public class ExceptionResult
    {
        public ExceptionResult(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
        public object CancelExceptionAndReturn { get; set; }
    }
}