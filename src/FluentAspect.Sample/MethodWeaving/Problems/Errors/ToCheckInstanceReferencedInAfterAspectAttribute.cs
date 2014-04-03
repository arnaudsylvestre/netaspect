using System;

namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckInstanceReferencedInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref ToCheckInstanceReferencedInAfter instance)
        {
        }
    }
}