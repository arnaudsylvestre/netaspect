﻿using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckNotRenameInAssemblyAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}