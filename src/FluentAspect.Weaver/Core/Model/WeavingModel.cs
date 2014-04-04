using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Call;
using NetAspect.Weaver.Core.Weaver.Method;

namespace NetAspect.Weaver.Core.Model
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
        }

        public MethodWeavingModel Method { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Method.Afters.Count == 0 &&
                       Method.Befores.Count == 0 &&
                       Method.OnExceptions.Count == 0 &&
                       Method.OnFinallys.Count == 0 &&
                       Instructions.Count == 0;
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