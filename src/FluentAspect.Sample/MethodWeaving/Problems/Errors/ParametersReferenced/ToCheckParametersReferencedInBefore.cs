using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckParametersReferencedInBefore
    {
        [ToCheckParametersReferencedInBeforeAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckParametersReferencedInBeforeAspectAttribute : Attribute
    {

        private string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}