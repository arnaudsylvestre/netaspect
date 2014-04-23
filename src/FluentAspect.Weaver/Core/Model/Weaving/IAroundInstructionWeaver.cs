using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public interface IAroundInstructionWeaver
   {
      void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables);
      void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables);
   }
}