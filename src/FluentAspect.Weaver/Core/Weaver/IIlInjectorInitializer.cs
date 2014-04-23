using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver
{
   public interface IIlInjectorInitializer<T>
   {
      void Inject(AroundInstructionIl aroundInstructionIl, T availableInformations, Instruction instruction);
   }
}