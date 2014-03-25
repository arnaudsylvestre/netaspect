using System;
using FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
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