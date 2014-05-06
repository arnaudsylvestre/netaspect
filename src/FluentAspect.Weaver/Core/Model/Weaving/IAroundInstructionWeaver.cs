using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public interface IAroundInstructionWeaver
   {
      void Weave(AroundInstructionIl il, IlInjectorAvailableVariables variables);
      void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables variables);
   }
}