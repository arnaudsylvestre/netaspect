using System;
using System.Reflection;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckMethodReferencedInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref MethodInfo method)
        {
        }
    }
}