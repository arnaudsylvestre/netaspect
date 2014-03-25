using System;
using FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
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