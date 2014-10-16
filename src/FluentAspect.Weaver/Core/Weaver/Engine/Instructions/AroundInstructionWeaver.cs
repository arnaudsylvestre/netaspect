using System;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionWeaver
   {
       public CustomAttribute Instance { get; private set; }
       public NetAspectDefinition Aspect { get; private set; }
       private readonly IIlInjector<VariablesForInstruction> after;
       private readonly IIlInjector<VariablesForInstruction> before;

       public AroundInstructionWeaver(CustomAttribute instance, NetAspectDefinition aspect, IIlInjector<VariablesForInstruction> before, IIlInjector<VariablesForInstruction> after)
      {
           Instance = instance;
           Aspect = aspect;
           this.before = before;
         this.after = after;
      }


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
