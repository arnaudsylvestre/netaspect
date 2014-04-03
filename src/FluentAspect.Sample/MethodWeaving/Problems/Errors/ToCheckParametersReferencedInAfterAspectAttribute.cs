using System;
using NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}