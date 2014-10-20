using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Session
{
   public interface IInstructionAspectInstanceDetector
   {
       IEnumerable<AspectInstanceForInstruction> GetAspectInstances(MethodDefinition method, Mono.Cecil.Cil.Instruction instruction, NetAspectDefinition aspect);
   }
}
