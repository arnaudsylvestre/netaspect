using System;
using NetAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedOnExceptionAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void OnException(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}