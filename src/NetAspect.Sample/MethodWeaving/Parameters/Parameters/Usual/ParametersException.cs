using System;

namespace NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ParametersException : Exception
    {
        public ParametersException(object[] parameters)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; set; }
    }
}