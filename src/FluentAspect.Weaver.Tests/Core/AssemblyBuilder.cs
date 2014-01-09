using System;
using System.Linq;
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

    public class TypeDefinitionDefiner
    {
        private TypeDefinition type;

        public TypeDefinitionDefiner(TypeDefinition type)
        {
            this.type = type;
        }

        public MethodDefinitionDefiner WithMethod(string methodName)
        {
            var methodDefinition = new MethodDefinition(methodName, MethodAttributes.Public, type.Module.Import(typeof (void)));
            type.Methods.Add(methodDefinition);
            return new MethodDefinitionDefiner(methodDefinition);
        }
    }

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
            var typeDefinition = CreateType(typeName);
            return new TypeDefinitionDefiner(typeDefinition);
        }

        private TypeDefinition CreateType(string typeName)
        {
            var typeDefinition = new TypeDefinition(DefaultNamespace, typeName, TypeAttributes.Class);
            _assemblyDefinition.MainModule.Types.Add(typeDefinition);
            return typeDefinition;
        }

        public CallWeavingAspectDefiner WithCallWeavingAspect(string typeName)
        {
            return new CallWeavingAspectDefiner(CreateType(typeName));
        }

        public MethodWeavingAspectDefiner WithMethodWeavingAspect(string typeName)
        {
            return new MethodWeavingAspectDefiner(CreateType(typeName));
        }

        public void Save(string filename)
        {
            _assemblyDefinition.Write(filename);
        }
    }

    public class MethodWeavingAspectDefiner
    {
        private readonly TypeDefinition typeDefinition;

        public MethodWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof (Attribute));
            typeDefinition.Methods.Add(new MethodDefinition(".ctor", MethodAttributes.Public, typeDefinition.Module.Import(typeof(void))));
        }

        public MethodReference Constructor
        {
            get { return (from m in typeDefinition.Methods where m.Name == ".ctor" select m).First(); }
        }

        public MethodDefinitionDefiner AddBefore()
        {
            var definition = new MethodDefinition("Before", MethodAttributes.Public, typeDefinition.Module.Import(typeof(void)));
            typeDefinition.Methods.Add(definition);
            return new MethodDefinitionDefiner(definition);
        }
    }

    public class CallWeavingAspectDefiner
    {
        private TypeDefinition typeDefinition;

        public CallWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof(Attribute));
        }

        public MethodDefinitionDefiner AddBeforeCall()
        {
            var definition = new MethodDefinition("BeforeCall", MethodAttributes.Public, typeDefinition.Module.Import(typeof (object)));
            typeDefinition.Methods.Add(definition);
            return new MethodDefinitionDefiner(definition);
        }
    }

    public class MethodDefinitionDefiner
    {
        private readonly MethodDefinition _definition;

        public MethodDefinitionDefiner(MethodDefinition definition)
        {
            _definition = definition;
        }

        public MethodDefinitionDefiner WithParameter<T>(string name)
        {
            _definition.Parameters.Add(new ParameterDefinition(name, ParameterAttributes.None,
                                                                   _definition.Module.Import(typeof (T))));
            return this;
        }

        public void AddAspect(MethodWeavingAspectDefiner aspect)
        {
            _definition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }
    }
}