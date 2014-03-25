using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckMethodWithWrongTypeInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(int method)
        {
        }
    }
}