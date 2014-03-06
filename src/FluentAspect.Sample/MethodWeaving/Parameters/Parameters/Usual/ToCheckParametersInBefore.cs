using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInBefore
    {
        [ToCheckParametersInBeforeAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }

    public class ToCheckParametersInBeforeAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }

    public class ParametersException : Exception
    {
        public ParametersException(object[] parameters)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; set; }
    }
}