using System;

namespace NetAspect.Sample
{
    public class CheckOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall()
        {
            throw new NotSupportedException();
        }
    }
}