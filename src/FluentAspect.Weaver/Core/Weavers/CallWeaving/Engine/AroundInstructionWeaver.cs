using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public interface ICallWeavingProvider
    {
        void AddBefore(List<Instruction> beforeInstructions);
        void AddAfter(List<Instruction> beforeInstructions);
        void Check(ErrorHandler error);
    }

    public class AroundInstructionWeaver : IWeaveable
    {
        private JoinPoint point;
        private ICallWeavingProvider provider;

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
            point.Method.InsertBefore(point.Instruction, instructions);
        }

        public void Check(ErrorHandler error)
        {
            provider.Check(error);
        }

        private void InsertAfterJointPointInstructions(ICallWeavingProvider provider)
        {
            var afterInstructions = new List<Instruction>();
            provider.AddAfter(afterInstructions);
            point.Method.InsertAfter(point.Instruction, afterInstructions);
        }
    }
}