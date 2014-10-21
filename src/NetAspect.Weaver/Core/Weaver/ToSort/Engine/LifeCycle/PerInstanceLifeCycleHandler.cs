using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle
{
   public class PerInstanceLifeCycleHandler : ILifeCycleHandler
   {
      public bool Static { get; set; }

      public void CreateInterceptor(Type aspectType,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Mono.Cecil.Cil.Instruction> instructions, CustomAttribute attribute)
      {
          TypeReference fieldType = method_P.Module.Import(aspectType);
         var fieldDefinition = new FieldDefinition(Guid.NewGuid().ToString(), FieldAttributes.Private, fieldType)
         {
            IsStatic = Static
         };
         method_P.DeclaringType.Fields.Add(fieldDefinition);

         Mono.Cecil.Cil.Instruction end = Mono.Cecil.Cil.Instruction.Create(OpCodes.Nop);

         if (!Static) instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(Static ? OpCodes.Ldsfld : OpCodes.Ldfld, fieldDefinition));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Brtrue, end));
         instructions.AppendCreateNewObject(interceptorVariable, aspectType, method_P.Module, attribute);
         if (!Static) instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, interceptorVariable));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(Static ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
         instructions.Add(end);
         if (!Static) instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(Static ? OpCodes.Ldsfld : OpCodes.Ldfld, fieldDefinition));
         instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, interceptorVariable));
      }
   }
}
