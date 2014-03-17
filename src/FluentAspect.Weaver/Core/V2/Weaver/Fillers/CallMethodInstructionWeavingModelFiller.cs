using System;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.V2.Model;
using FluentAspect.Weaver.Core.V2.Weaver.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Fillers
{
    public class CallMethodInstructionWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeCallMethod.Method != null ||
                aspect.AfterCallMethod.Method != null ||
                aspect.AfterRaiseEvent.Method != null ||
                aspect.BeforeRaiseEvent.Method != null;
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
                if (IsEventCall(instruction, aspect, method))
                {
                    throw new NotImplementedException();
                }
            }
        }

        private bool IsEventCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            try
            {
                if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldsfld)
            {
                var next = instruction.Next;

                var eventReference = instruction.Operand as FieldReference;

                if (next.OpCode == OpCodes.Call || next.OpCode == OpCodes.Calli || next.OpCode == OpCodes.Callvirt)
                {
                    var methodReference = next.Operand as MethodReference;
                    if (methodReference.Name == "Invoke" && methodReference.DeclaringType.Name == "Action")
                    {
                        var eventDefinition = method.DeclaringType.Events.First(e => e.Name == eventReference.Name);
                        var aspectType = method.Module.Import(aspect.Type);
                        return eventDefinition.CustomAttributes.Any(
                            customAttribute_L =>
                            customAttribute_L.AttributeType.FullName == aspectType.FullName);
                    }
                }
                return false;
            }
            }
            catch (Exception)
            {
            }
            return false;
            
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
}