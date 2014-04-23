using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionWeaver : IAroundInstructionWeaver
   {
      private IIlInjector<IlInjectorAvailableVariablesForInstruction> before;
      private IIlInjector<IlInjectorAvailableVariablesForInstruction> after;

      public AroundInstructionWeaver(IIlInjector<IlInjectorAvailableVariablesForInstruction> before, IIlInjector<IlInjectorAvailableVariablesForInstruction> after)
      {
         this.before = before;
         this.after = after;
      }

      public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables)
      {
         before.Check(errorHandler, variables);
         after.Check(errorHandler, variables);
      }

      public void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables)
      {
         before.Inject(il.BeforeInstruction, variables);
         after.Inject(il.AfterInstruction, variables);
      }
   }
}