using System;

namespace FluentAspect.Sample.AOP
{
    internal class CheckWithReturnAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref string result)
        {
            result = "Weaved";
        }
    }
}