using System;

namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckMethodWithWrongTypeInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(int method)
        {
        }
    }
}