using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration
{
    public class AspectMatch<T>
    {
        public IEnumerable<AssemblyDefinition> AssembliesToScan { get; set; }

        public Func<T, bool> Matcher { get; set; }

        public NetAspectDefinition Aspect { get; set; }
        
    }
}