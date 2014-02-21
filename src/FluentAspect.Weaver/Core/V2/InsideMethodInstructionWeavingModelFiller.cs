using System;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class InsideMethodInstructionWeavingModelFiller : IWeavingModelFiller
    {
        private Func<Instruction, NetAspectDefinition, bool> instructionCompliant;

        public InsideMethodInstructionWeavingModelFiller(Func<Instruction, NetAspectDefinition, bool> instructionCompliant_P)
        {
            instructionCompliant = instructionCompliant_P;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            if (!method.Body.Instructions.Any(instruction_L => instructionCompliant(instruction_L, aspect)))
                return;
        }


    }
}