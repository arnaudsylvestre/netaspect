using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class WeavingModel
    {
        public WeavingModel()
        {
            Method = new MethodWeavingModel();
            Instructions = new Dictionary<Instruction, IInstructionIlInjector>();
        }

        public MethodWeavingModel Method { get; set; }
        public Dictionary<Instruction, IInstructionIlInjector> Instructions { get; set; }

        public bool IsEmpty
        {
            get { return Method.Afters.Count == 0 && 
                         Method.Befores.Count == 0 && 
                         Method.OnExceptions.Count == 0 && 
                         Method.OnFinallys.Count == 0 && Instructions.Count == 0; }
        }
    }
}