using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckResultInVoid
    {
        [ToCheckResultInVoidAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }

    public class ToCheckResultInVoidAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
        }
    }
}