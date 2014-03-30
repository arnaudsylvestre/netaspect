using System.Collections;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weaver;
using FluentAspect.Weaver.Core.Weaver.Call;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Model
{
    public class WeavingModel
    {
        public readonly Dictionary<Instruction, List<IAroundInstructionWeaver>> Instructions = new Dictionary<Instruction, List<IAroundInstructionWeaver>>();

        public void AddAroundInstructionWeaver(Instruction instruction, IAroundInstructionWeaver weaver)
        {
            if (!Instructions.ContainsKey(instruction))
                Instructions.Add(instruction, new List<IAroundInstructionWeaver>());
            Instructions[instruction].Add(weaver);
        }

        public WeavingModel()
        {
            Method = new MethodWeavingModel();
            BeforeInstructions = new Dictionary<Instruction, IIlInjector<IlInstructionInjectorAvailableVariables>>();
            AfterInstructions = new Dictionary<Instruction, IIlInjector<IlInstructionInjectorAvailableVariables>>();
        }

        public MethodWeavingModel Method { get; set; }
        public Dictionary<Instruction, IIlInjector<IlInstructionInjectorAvailableVariables>> BeforeInstructions { get; set; }
        public Dictionary<Instruction, IIlInjector<IlInstructionInjectorAvailableVariables>> AfterInstructions { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Method.Afters.Count == 0 &&
                       Method.Befores.Count == 0 &&
                       Method.OnExceptions.Count == 0 &&
                       Method.OnFinallys.Count == 0 &&
                       BeforeInstructions.Count == 0 &&
                       AfterInstructions.Count == 0;
            }
        }

        public IEnumerable<IAroundInstructionWeaver> GetAroundInstructionWeavers(Instruction instruction)
        {
            if (!Instructions.ContainsKey(instruction))
                return new List<IAroundInstructionWeaver>();
            return Instructions[instruction];
        }
    }
}