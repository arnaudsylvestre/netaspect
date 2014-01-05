using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckParametersReferencedOnException
    {
        [ToCheckParametersReferencedOnExceptionAspect]
        public void Check(string parameter1, int parameter2)
        {
            throw new Exception();
        }
    }

    public class ToCheckParametersReferencedOnExceptionAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void OnException(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}