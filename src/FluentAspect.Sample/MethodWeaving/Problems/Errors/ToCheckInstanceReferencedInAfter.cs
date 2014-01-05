using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckInstanceReferencedInAfter
    {
        [ToCheckInstanceReferencedInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckInstanceReferencedInAfterAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref ToCheckInstanceReferencedInAfter instance)
        {

        }
    }
}