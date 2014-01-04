using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
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

        private string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }

    public class ParametersException : Exception
    {
        public object[] Parameters { get; set; }

        public ParametersException(object[] parameters)
        {
            Parameters = parameters;
        }
    }
}