using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Tests.Core
{
    public class MethodDefinitionDefiner
    {
        private readonly MethodDefinition _definition;

        public MethodDefinitionDefiner(MethodDefinition definition)
        {
            _definition = definition;
        }

        public MethodDefinition Definition
        {
            get { return _definition; }
        }

        public MethodDefinitionDefiner WithParameter<T>(string name)
        {
            var parameterType = _definition.Module.Import(typeof(T));
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, parameterType);
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);
            int i = 0;
            if (!_definition.IsStatic)
                _definition.Body.Instructions.Insert(i++, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(i++, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(i++, Instruction.Create(_definition.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WithReferencedParameter<T>(string name)
        {
            var parameterType = _definition.Module.Import(typeof(T));
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, new ByReferenceType(parameterType));
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            //_definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldind_Ref));
            _definition.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stsfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WhichRaiseException()
        {
            var index = _definition.Body.Instructions.FindIndex(instruction => instruction.OpCode == OpCodes.Ret);

            _definition.Body.Instructions.Insert(index, Instruction.Create(OpCodes.Newobj, _definition.Module.Import(typeof(Exception).GetConstructor(new Type[0]))));
            _definition.Body.Instructions.Insert(index + 1, Instruction.Create(OpCodes.Throw));
            return this;
        }

        public void AddAspect(MethodWeavingAspectDefiner aspect)
        {
            _definition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }

        public MethodDefinitionDefiner WithParameter(string name, TypeDefinition parameterType)
        {
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, parameterType);
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            _definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WhichInstantiateAnObject(string tocall, TypeDefinitionDefiner type)
        {
            var variableDefinition = new VariableDefinition(tocall, type.Type);
            _definition.Body.Variables.Add(variableDefinition);
            _definition.Body.InitLocals = true;
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, type.Type.GetConstructor()));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
            return this;
        }

        public MethodDefinitionDefiner AndInitializeField(string variableName, string fieldName, int value)
        {
            var variable = _definition.Find(variableName);
            var field = ((TypeDefinition) variable.VariableType).FindField(fieldName);
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, variable));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, field));
            return this;
        }

        public MethodDefinitionDefiner AndReturn()
        {
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return this;
        }
    }
}