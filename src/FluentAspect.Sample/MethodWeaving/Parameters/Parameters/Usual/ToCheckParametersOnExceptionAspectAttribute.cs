using System;

namespace NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersOnExceptionAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void OnException(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}