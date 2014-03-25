using System;

namespace FluentAspect.Sample
{
    public class CheckOnCallBeforeAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void BeforeCall()
        {
            throw new NotSupportedException();
        }
    }
}