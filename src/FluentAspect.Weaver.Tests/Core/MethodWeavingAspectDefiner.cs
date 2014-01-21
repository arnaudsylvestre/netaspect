using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Tests.Core
{
    public class MethodWeavingAspectDefiner
    {
        private readonly TypeDefinition typeDefinition;
        private MethodDefinition DefaultConstructor;

        public TypeDefinition TypeDefinition
        {
            get { return typeDefinition; }
        }

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
            return CreateInterceptor(typeDefinition, "Before", false);
        }

        public MethodDefinitionDefiner AddAfter()
        {
            return CreateInterceptor(typeDefinition, "After", false);
        }

        public MethodDefinitionDefiner AddOnException()
        {
            return CreateInterceptor(typeDefinition, "OnException", false);
        }

        public static MethodDefinitionDefiner CreateInterceptor(TypeDefinition typeDefinition, string before, bool isStatic)
        {
            var firstOrDefault = (from e in typeDefinition.Methods where e.Name == before select e).FirstOrDefault();
            if (firstOrDefault != null)
                return new MethodDefinitionDefiner(firstOrDefault);

            var methodAttributes_L = MethodAttributes.Public;
            if (isStatic)
                methodAttributes_L |= MethodAttributes.Static;
            var definition = new MethodDefinition(before, methodAttributes_L, typeDefinition.Module.Import(typeof (void)));
            typeDefinition.Methods.Add(definition);
            definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return new MethodDefinitionDefiner(definition);
        }
    }
}