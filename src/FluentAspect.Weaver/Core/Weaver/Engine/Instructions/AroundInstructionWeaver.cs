using System;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionWeaver
   {
      private readonly IIlInjector<VariablesForInstruction> after;
       private readonly IIlInjector<VariablesForInstruction> before;

       public AroundInstructionWeaver(IIlInjector<VariablesForInstruction> before, IIlInjector<VariablesForInstruction> after, Type aspectType)
      {
           AspectType = aspectType;
           this.before = before;
         this.after = after;
      }

       public Type AspectType { get; private set; }

       public void Check(ErrorHandler errorHandler, VariablesForInstruction variables)
      {
          before.Check(errorHandler, variables);
          after.Check(errorHandler, variables);
      }

      public void Weave(AroundInstructionIl il, VariablesForInstruction variables)
      {
         before.Inject(il.BeforeInstruction, variables);
         after.Inject(il.AfterInstruction, variables);
      }
   }
}
