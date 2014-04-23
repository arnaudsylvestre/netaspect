using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionWeaver : IAroundInstructionWeaver
   {
      private IIlInjector before;
      private IIlInjector after;

      public AroundInstructionWeaver(IIlInjector before, IIlInjector after)
      {
         this.before = before;
         this.after = after;
      }

      public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables)
      {
         before.Check(errorHandler);
         after.Check(errorHandler);
      }

      public void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables)
      {
         before.Inject(il.BeforeInstruction, variables);
         after.Inject(il.AfterInstruction, variables);
      }
   }
}