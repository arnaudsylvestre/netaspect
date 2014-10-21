using System;

namespace NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersOnException
    {
        [ToCheckParametersOnExceptionAspect]
        public void Check(string parameter1, int parameter2)
        {
            throw new Exception();
        }
    }
}