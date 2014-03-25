using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckResultInVoidAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
        }
    }
}