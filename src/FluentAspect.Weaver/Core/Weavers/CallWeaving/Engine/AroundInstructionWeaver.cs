using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class AroundInstructionWeaver : IWeaveable
    {
        private readonly JoinPoint point;
        private readonly ICallWeavingProvider provider;

        public AroundInstructionWeaver(JoinPoint point, ICallWeavingProvider provider)
        {
            this.point = point;
            this.provider = provider;
        }

        public void Weave()
        {
            var instructions = new List<Instruction>();

            var beforeInstructions = new List<Instruction>();
            provider.AddBefore(beforeInstructions);
            instructions.AddRange(beforeInstructions);
            point.Method.Body.InitLocals = true;


            InsertAfterJointPointInstructions(provider);
            point.Method.InsertBefore(point.InstructionStart, instructions);
        }

        public void Check(ErrorHandler error)
        {
            provider.Check(error);
        }

        private void InsertAfterJointPointInstructions(ICallWeavingProvider provider)
        {
            var afterInstructions = new List<Instruction>();
            provider.AddAfter(afterInstructions);
            point.Method.InsertAfter(point.InstructionEnd, afterInstructions);
        }
    }
}