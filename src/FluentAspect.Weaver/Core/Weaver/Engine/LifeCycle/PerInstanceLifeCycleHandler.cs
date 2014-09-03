using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class PerInstanceLifeCycleHandler : ILifeCycleHandler
   {
      public bool Static { get; set; }

      public void CreateInterceptor(NetAspectDefinition aspect_P,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Instruction> instructions)
      {
         TypeReference fieldType = method_P.Module.Import(aspect_P.Type);
         var fieldDefinition = new FieldDefinition(Guid.NewGuid().ToString(), FieldAttributes.Private, fieldType)
         {
            IsStatic = Static
         };
         method_P.DeclaringType.Fields.Add(fieldDefinition);

         Instruction end = Instruction.Create(OpCodes.Nop);

         if (!Static) instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Instruction.Create(Static ? OpCodes.Ldsfld : OpCodes.Ldfld, fieldDefinition));
         instructions.Add(Instruction.Create(OpCodes.Brtrue, end));
         instructions.AppendCreateNewObject(interceptorVariable, aspect_P.Type, method_P.Module);
         if (!Static) instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptorVariable));
         instructions.Add(Instruction.Create(Static ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
         instructions.Add(end);
         if (!Static) instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
         instructions.Add(Instruction.Create(Static ? OpCodes.Ldsfld : OpCodes.Ldfld, fieldDefinition));
         instructions.Add(Instruction.Create(OpCodes.Stloc, interceptorVariable));
      }
   }
}
