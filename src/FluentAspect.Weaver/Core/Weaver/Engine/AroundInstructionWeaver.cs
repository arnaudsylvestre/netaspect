using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class AroundInstructionWeaver : IAroundInstructionWeaver
   {
      private IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction> initializer;
      private IIlInjector<IlInjectorAvailableVariablesForInstruction> before;
      private IIlInjector<IlInjectorAvailableVariablesForInstruction> after;

      public AroundInstructionWeaver(IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction> initializer, IIlInjector<IlInjectorAvailableVariablesForInstruction> before, IIlInjector<IlInjectorAvailableVariablesForInstruction> after)
      {
         this.initializer = initializer;
         this.before = before;
         this.after = after;
      }

      public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables)
      {
         before.Check(errorHandler, variables);
         after.Check(errorHandler, variables);
      }

      public void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction)
      {
         initializer.Inject(il, variables, instruction);
         before.Inject(il.BeforeInstruction, variables);
         after.Inject(il.AfterInstruction, variables);
      }
   }
}