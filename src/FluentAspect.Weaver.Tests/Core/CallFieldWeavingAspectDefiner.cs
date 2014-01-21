using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace FluentAspect.Weaver.Tests.Core
{
    public class CallFieldWeavingAspectDefiner
    {
        private readonly TypeDefinition _typeDefinition;

        public CallFieldWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            _typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof(Attribute));
            var fieldDefinition_L = new FieldDefinition("NetAspectAttributeKind", FieldAttributes.Public, typeDefinition.Module.TypeSystem.String);
            typeDefinition.Fields.Add(fieldDefinition_L);

            var instructions_L = new List<Instruction>();
            instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions_L.Add(Instruction.Create(OpCodes.Ldstr, "CallWeaving"));
            instructions_L.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition_L));

            var constructorInfo_L = typeof(Attribute).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DefaultConstructor = MethodWeavingAspectDefiner.AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), instructions_L);
        }

        protected MethodDefinition DefaultConstructor { get; set; }

        public MethodDefinition Constructor
        {
            get { return DefaultConstructor; }
        }

        public MethodDefinitionDefiner AddBeforeFieldAccess()
        {
            return MethodWeavingAspectDefiner.CreateInterceptor(_typeDefinition, "BeforeUpdateFieldValue", true);
        }

        public MethodDefinitionDefiner AddAfterFieldAccess()
        {
            return MethodWeavingAspectDefiner.CreateInterceptor(_typeDefinition, "AfterUpdateFieldValue", true);
        }

    }
}