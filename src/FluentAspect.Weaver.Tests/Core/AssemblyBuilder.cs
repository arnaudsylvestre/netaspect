using System;
using Mono.Cecil;

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

    public class AssemblyDefinitionDefiner
    {
        private const string DefaultNamespace = "A";

        private readonly AssemblyDefinition _assemblyDefinition;

        public AssemblyDefinitionDefiner(AssemblyDefinition assemblyDefinition)
        {
            _assemblyDefinition = assemblyDefinition;
        }

        public MethodDefinitionDefinber WithMethod(string typeName, string methodName)
        {
            var typeDefinition = _assemblyDefinition.MainModule.GetType(DefaultNamespace, typeName);
            if (typeDefinition == null)
                _assemblyDefinition.MainModule.Types.Add(new TypeDefinition(DefaultNamespace, ));
        }

        public CallWeavingAspectDefiner WithCallWeavingAspect()
        {
            
        }
    }
}