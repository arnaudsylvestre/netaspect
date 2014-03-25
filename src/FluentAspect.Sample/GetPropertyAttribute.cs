using System;

namespace FluentAspect.Sample
{
    public class GetPropertyAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref string result)
        {
            result = "3";
        }
    }
}