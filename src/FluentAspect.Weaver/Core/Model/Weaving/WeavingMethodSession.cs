using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public static class WeavingMethodSessionExtensions
    {

        public static void AddAroundInstructionWeaver(this WeavingMethodSession session, Instruction instruction, IEnumerable<AspectInstanceForInstruction> weaver)
        {
            if (!session.Instructions.ContainsKey(instruction))
                session.Instructions.Add(instruction, new List<AspectInstanceForInstruction>());
            session.Instructions[instruction].AddRange(weaver);
        }
    }

   public class WeavingMethodSession
   {
       public readonly Dictionary<Instruction, List<AspectInstanceForInstruction>> Instructions = new Dictionary<Instruction, List<AspectInstanceForInstruction>>();

       public readonly List<AspectInstanceForMethodWeaving> Method = new List<AspectInstanceForMethodWeaving>();

   }
}
