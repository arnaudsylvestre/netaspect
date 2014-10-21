using System;
using NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedInBeforeAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}