using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.IL
{
    public static class MethodDefinitionExtensions
    {
        public static void FillArgsArrayFromParameters(this MethodDefinition definition, List<Instruction> instructions, VariableDefinition args)
        {
            if (args == null)
                return;
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, definition.Parameters.Count));
            instructions.Add(Instruction.Create(OpCodes.Newarr, definition.Module.Import(typeof (object))));
            instructions.Add(Instruction.Create(OpCodes.Stloc, args));

            foreach (ParameterDefinition p in definition.Parameters.ToArray())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, args));
                instructions.Add(Instruction.Create(OpCodes.Ldc_I4, p.Index));
                instructions.Add(Instruction.Create(OpCodes.Ldarg, p));
                if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
                    instructions.Add(Instruction.Create(OpCodes.Box, p.ParameterType));
                instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }
        }

        public static void FillCalledArgsArrayFromParameters(this MethodDefinition calledMethod, List<Instruction> instructions, VariableDefinition args, Dictionary<string, VariableDefinition> variablesParameters)
        {
            if (args == null)
                return;
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, calledMethod.Parameters.Count));
            instructions.Add(Instruction.Create(OpCodes.Newarr, calledMethod.Module.Import(typeof(object))));
            instructions.Add(Instruction.Create(OpCodes.Stloc, args));

            foreach (ParameterDefinition p in calledMethod.Parameters.ToArray())
            {
               instructions.Add(Instruction.Create(OpCodes.Ldloc, args));
               instructions.Add(Instruction.Create(OpCodes.Ldc_I4, p.Index));
                instructions.Add(Instruction.Create(OpCodes.Ldloc, variablesParameters["called" + p.Name.ToLower()]));
                if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
                    instructions.Add(Instruction.Create(OpCodes.Box, p.ParameterType));
                instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }
        }
    }

    
}