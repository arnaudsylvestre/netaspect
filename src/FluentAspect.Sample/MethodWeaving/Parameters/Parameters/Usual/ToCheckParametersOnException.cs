using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckParametersOnException
    {
        [ToCheckParametersOnExceptionAspect]
        public void Check(string parameter1, int parameter2)
        {
            throw new Exception();
        }
    }

    public class ToCheckParametersOnExceptionAspectAttribute : Attribute
    {

        private string NetAspectAttributeKind = "MethodWeaving";

        public void OnException(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}