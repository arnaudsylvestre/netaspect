using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckParametersReferencedInAfter
    {
        [ToCheckParametersReferencedInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckParametersReferencedInAfterAspectAttribute : Attribute
    {

        private string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}