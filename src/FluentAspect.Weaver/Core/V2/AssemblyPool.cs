using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class AssemblyPool
    {
        Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public void Add(Assembly assembly)
        {
            asms.Add(assembly, AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(), new ReaderParameters(ReadingMode.Immediate)
                {
                    ReadSymbols = true
                }));
        }

        public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
        {
            return asms[assembly];
        }

        public void Save(ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            foreach (var def in asms)
            {
                WeaverCore.WeaveOneAssembly(def.Key.GetAssemblyPath(), def.Value, errorHandler, newAssemblyNameProvider);
            }
        }
    }
}