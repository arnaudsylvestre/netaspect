using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckNotRenameInAssemblyAttribute : Attribute
    {
        private string NetAspectAttributeKind = "MethodWeaving";
    }
}