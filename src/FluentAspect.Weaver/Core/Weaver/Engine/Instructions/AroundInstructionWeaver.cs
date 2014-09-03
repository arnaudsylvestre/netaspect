using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Engine.LifeCycle;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionWeaver
   {
      private readonly IIlInjector after;
      private readonly NetAspectDefinition aspect;
      private readonly AspectBuilder aspectBuilder;
      private readonly IIlInjector before;
      private readonly MethodDefinition method;

      public AroundInstructionWeaver(IIlInjector before, IIlInjector after, AspectBuilder aspectBuilder, NetAspectDefinition aspect, MethodDefinition method)
      {
         this.before = before;
         this.after = after;
         this.aspectBuilder = aspectBuilder;
         this.aspect = aspect;
         this.method = method;
      }

      public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables variables)
      {
         before.Check(errorHandler);
         after.Check(errorHandler);
      }

      public void Weave(AroundInstructionIl il, IlInjectorAvailableVariables variables)
      {
         before.Inject(il.BeforeInstruction, variables);
         after.Inject(il.AfterInstruction, variables);
      }

      public VariableDefinition CreateAspect(AroundInstructionIl il)
      {
         var interceptorVariable = new VariableDefinition(method.Module.Import(aspect.Type));
         aspectBuilder.CreateInterceptor(aspect, method, interceptorVariable, il.BeforeInstruction);
         return interceptorVariable;
      }
   }
}
