using System;

namespace FluentAspect.Sample
{
    public class CheckParametersCalledOnCallAfter : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string parameter1Called, string parameter2Called)
        {
            throw new Exception(parameter1Called + " " + parameter2Called);
        }
    }
}