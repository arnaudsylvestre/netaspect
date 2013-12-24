using System;
using System.Reflection;

namespace FluentAspect.Sample
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckNotRenameInAssemblyNetAspectAttribute : Attribute
    {
    }
}