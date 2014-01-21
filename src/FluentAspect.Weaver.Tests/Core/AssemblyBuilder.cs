using System;
using FluentAspect.Weaver.Tests.acceptance;
using Mono.Cecil;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Core
{
    public class AssemblyBuilder
    {
        public static AssemblyDefinitionDefiner Create()
        {
            return Create("Temp");
        }
        public static AssemblyDefinitionDefiner Create(string name)
        {
            return new AssemblyDefinitionDefiner(AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(name, new Version("1.0")), name, ModuleKind.Dll));
        }
    }
}