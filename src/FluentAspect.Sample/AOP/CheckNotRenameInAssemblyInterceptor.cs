using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckNotRenameInAssemblyNetAspectAttribute : Attribute
    {
    }
}