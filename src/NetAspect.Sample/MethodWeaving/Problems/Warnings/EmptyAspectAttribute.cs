using System;

namespace NetAspect.Sample.MethodWeaving.Problems.Warnings
{
    public class EmptyAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}