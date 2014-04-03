﻿using System;
using System.Reflection;

namespace NetAspect.Weaver.Helpers
{
    public static class AssemblyExtensions
    {
        public static string GetAssemblyPath(this Assembly assembly)
        {
            return Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
        }
    }
}