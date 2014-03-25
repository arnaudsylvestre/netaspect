using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedOnException
    {
        [ToCheckParametersReferencedOnExceptionAspect]
        public void Check(string parameter1, int parameter2)
        {
            throw new Exception();
        }
    }
}