using System;

namespace NetAspect.Sample
{
    public class CheckReturnSimpleTypeAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
            result = 5;
        }
    }
}