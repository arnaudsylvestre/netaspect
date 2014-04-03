using System;

namespace NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInBeforeAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}