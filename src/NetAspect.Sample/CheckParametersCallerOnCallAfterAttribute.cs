using System;

namespace NetAspect.Sample
{
    public class CheckParametersCallerOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string callerMethodParameterCaller)
        {
            throw new Exception(callerMethodParameterCaller);
        }
    }
}