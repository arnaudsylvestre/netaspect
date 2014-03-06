using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class WeavingModel
    {
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
                       Method.OnFinallys.Count == 0 && BeforeInstructions.Count == 0 && AfterInstructions.Count == 0;
            }
        }
    }
}