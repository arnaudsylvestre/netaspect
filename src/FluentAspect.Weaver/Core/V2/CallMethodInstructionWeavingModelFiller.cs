﻿using System;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class CallMethodInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeCallMethod.Method != null ||
                aspect.AfterCallMethod.Method != null;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (Instruction instruction in method.Body.Instructions)
            {
                if (IsMethodCall(instruction, aspect, method))
                {
                    weavingModel.AddMethodCallWeavingModel(method, instruction, aspect, aspect.BeforeCallMethod,
                                                           aspect.AfterCallMethod);
                }
            }
        }

        private static bool IsMethodCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Calli ||
                instruction.OpCode == OpCodes.Callvirt)
            {
                var methodReference = instruction.Operand as MethodReference;

                TypeReference aspectType = method.Module.Import(aspect.Type);
                bool compliant =
                    methodReference.Resolve()
                                   .CustomAttributes.Any(
                                       customAttribute_L =>
                                       customAttribute_L.AttributeType.FullName == aspectType.FullName);
                return compliant;
            }
            return false;
        }
    }

    public class CallGetFieldInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            throw new NotImplementedException();
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            if (!method.Body.Instructions.Any(instruction_L => IsFieldCall(instruction_L, aspect, method)))
                return;
            throw new NotImplementedException();
        }

        private static bool IsFieldCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda ||
                instruction.OpCode == OpCodes.Ldsfld ||
                instruction.OpCode == OpCodes.Ldsflda)
            {
                var fieldReference = instruction.Operand as FieldReference;

                TypeReference aspectType = method.Module.Import(aspect.Type);
                bool compliant =
                    fieldReference.Resolve()
                                  .CustomAttributes.Any(
                                      customAttribute_L =>
                                      customAttribute_L.AttributeType.FullName == aspectType.FullName);
                return compliant;
            }
            return false;
        }
    }
}