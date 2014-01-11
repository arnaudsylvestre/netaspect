using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

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
            var methodDefinition = new MethodDefinition(methodName, MethodAttributes.Public, type.Module.TypeSystem.Void);
           methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
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
    }

    public class MethodWeavingAspectDefiner
    {
        private readonly TypeDefinition typeDefinition;
       private MethodDefinition DefaultConstructor;

        public MethodWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof (Attribute));
           var fieldDefinition_L = new FieldDefinition("NetAspectAttributeKind", FieldAttributes.Public, typeDefinition.Module.TypeSystem.String);
           typeDefinition.Fields.Add(fieldDefinition_L);

           var instructions_L = new List<Instruction>();
           instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
           instructions_L.Add(Instruction.Create(OpCodes.Ldstr, "MethodWeaving"));
           instructions_L.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition_L));

           var constructorInfo_L = typeof (Attribute).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
           DefaultConstructor = AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), instructions_L);
        }

        public static MethodDefinition AddEmptyConstructor(TypeDefinition type, MethodReference baseEmptyConstructor, IEnumerable<Instruction> instructions)
        {
           var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
           var method = new MethodDefinition(".ctor", methodAttributes, type.Module.TypeSystem.Void);
           foreach (var instruction in instructions)
           {
              method.Body.Instructions.Add(instruction);
           }
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, baseEmptyConstructor));
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
           type.Methods.Add(method);
           return method;
        }

        public MethodReference Constructor
        {
           get { return DefaultConstructor; }
        }

        public MethodDefinitionDefiner AddBefore()
        {
            var definition = new MethodDefinition("Before", MethodAttributes.Public, typeDefinition.Module.Import(typeof(void)));
            typeDefinition.Methods.Add(definition);
           definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return new MethodDefinitionDefiner(definition);
        }
    }

    public class CallWeavingAspectDefiner
    {
        private TypeDefinition typeDefinition;

        public CallWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
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
            var parameterType = _definition.Module.Import(typeof (T));
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, parameterType);
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition("Before" + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            _definition.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(1,Instruction.Create(OpCodes.Ldarg, parameterDefinition));
           _definition.Body.Instructions.Insert(2,Instruction.Create(OpCodes.Stfld, fieldDefinition));
            return this;
        }

        public void AddAspect(MethodWeavingAspectDefiner aspect)
        {
            _definition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }
    }
}