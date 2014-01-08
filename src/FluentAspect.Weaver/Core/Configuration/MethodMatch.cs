using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration
{
    public class MethodMatch
    {
        public IEnumerable<AssemblyDefinition> AssembliesToScan { get; set; }

        public Func<IMethod, bool> Matcher { get; set; }

        public MethodWeavingConfiguration MethodWeavingInterceptors { get; set; }
        public CallWeavingConfiguration CallWeavingInterceptors { get; set; }
    }
}