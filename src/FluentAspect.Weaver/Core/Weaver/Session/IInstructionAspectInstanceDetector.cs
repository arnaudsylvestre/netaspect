using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public interface IInstructionAspectInstanceDetector
   {
       IEnumerable<AspectInstanceForInstruction> GetAspectInstances(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect);
   }
}
