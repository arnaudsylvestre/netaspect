using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public interface IAroundInstructionWeaver
   {
      void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction);
      void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables);
   }
}