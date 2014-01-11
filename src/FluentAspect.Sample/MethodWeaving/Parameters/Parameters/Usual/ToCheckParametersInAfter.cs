using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInAfter
    {
        [ToCheckParametersInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckParametersInAfterAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}