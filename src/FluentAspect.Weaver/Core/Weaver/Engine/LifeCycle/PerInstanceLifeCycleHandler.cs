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
        public void CreateInterceptor(NetAspectDefinition aspect_P, MethodDefinition method_P, VariableDefinition interceptorVariable,
                                     List<Instruction> instructions)
        {
            var fieldType = method_P.Module.Import(aspect_P.Type);
            var fieldDefinition = new FieldDefinition(Guid.NewGuid().ToString(), FieldAttributes.Private, fieldType);
            method_P.DeclaringType.Fields.Add(fieldDefinition);

            var end = Instruction.Create(OpCodes.Nop);

            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldfld, fieldDefinition));
            instructions.Add(Instruction.Create(OpCodes.Brtrue, end));
            instructions.AppendCreateNewObject(interceptorVariable, aspect_P.Type, method_P.Module);
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptorVariable));
            instructions.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition));
            instructions.Add(end);
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldfld, fieldDefinition));
            instructions.Add(Instruction.Create(OpCodes.Stloc, interceptorVariable));
        } 
    }
}