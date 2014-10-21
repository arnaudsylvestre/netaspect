using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace NetAspect.Weaver.Helpers.Mono.Cecil.IL
{
   public static class MethodDefinitionExtensions
   {
      public static PropertyDefinition GetPropertyForGetter(this MethodDefinition getMethod)
      {
         Collection<PropertyDefinition> properties_L = getMethod.DeclaringType.Properties;
         return properties_L.FirstOrDefault(property_L => property_L.GetMethod == getMethod);
      }

      public static PropertyDefinition GetPropertyForSetter(this MethodDefinition setMethod)
      {
         Collection<PropertyDefinition> properties_L = setMethod.DeclaringType.Properties;
         return properties_L.FirstOrDefault(property_L => property_L.SetMethod == setMethod);
      }

      public static PropertyDefinition GetProperty(this MethodDefinition method)
      {
         return GetPropertyForGetter(method) ?? GetPropertyForSetter(method);
      }

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
         instructions.Add(Instruction.Create(OpCodes.Newarr, calledMethod.Module.Import(typeof (object))));
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
