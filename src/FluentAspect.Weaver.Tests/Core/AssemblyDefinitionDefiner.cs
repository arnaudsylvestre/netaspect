using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace FluentAspect.Weaver.Tests.Core
{
    public class AssemblyDefinitionDefiner
    {
        public const string DefaultNamespace = "A";

        private readonly AssemblyDefinition _assemblyDefinition;

        public AssemblyDefinitionDefiner(AssemblyDefinition assemblyDefinition)
        {
            _assemblyDefinition = assemblyDefinition;
        }

        public TypeDefinitionDefiner WithType(string typeName)
        {
            var typeDefinition = CreateType(typeName, _assemblyDefinition.MainModule.TypeSystem.Object);
            var typeDefinitionDefiner_L = new TypeDefinitionDefiner(typeDefinition);
            var constructorInfo_L = typeof (object).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];

            MethodWeavingAspectDefiner.AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), new List<Instruction>());
            return typeDefinitionDefiner_L;
        }

        private TypeDefinition CreateType(string typeName, TypeReference parent)
        {
            var typeDefinition = new TypeDefinition(DefaultNamespace, typeName, TypeAttributes.Public/* | TypeAttributes.AutoClass*/, parent);
            _assemblyDefinition.MainModule.Types.Add(typeDefinition);
            return typeDefinition;
        }

        public CallWeavingAspectDefiner WithCallWeavingAspect(string typeName)
        {
            return new CallWeavingAspectDefiner(CreateType(typeName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }

        public MethodWeavingAspectDefiner WithMethodWeavingAspect(string typeName)
        {
            return new MethodWeavingAspectDefiner(CreateType(typeName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }

        public void Save(string filename)
        {
            _assemblyDefinition.Write(filename, new WriterParameters()
                {
                    WriteSymbols = true
                });
        }

        public CallFieldWeavingAspectDefiner WithCallFieldWeavingAspect(string aspectName)
        {
            return new CallFieldWeavingAspectDefiner(CreateType(aspectName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }
    }
}