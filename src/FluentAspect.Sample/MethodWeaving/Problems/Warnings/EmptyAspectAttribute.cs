using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Warnings
{
    public class EmptyAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}