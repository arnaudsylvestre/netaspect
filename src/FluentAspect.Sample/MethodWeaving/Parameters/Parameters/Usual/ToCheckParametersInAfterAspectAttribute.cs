using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}