using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public interface ILifeCycleHandler
   {
      void CreateInterceptor(AspectBuilder aspect_P, VariableDefinition interceptor);
   }

   public class TransientLifeCycleHandler : ILifeCycleHandler
   {
      public void CreateInterceptor(AspectBuilder aspect, VariableDefinition interceptor)
      {
         aspect.Instructions.AppendCreateNewObject(interceptor, aspect.Aspect.Type, aspect.Method.Module);
      }
   }

   public class AspectBuilder
   {
      private Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles;

      public List<VariableDefinition> Variables = new List<VariableDefinition>();
      public List<Instruction> Instructions = new List<Instruction>();

      

      public AspectBuilder(Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         lifeCycles = lifeCycles_P;
      }

      public void CreateInterceptor(NetAspectDefinition aspect_P, MethodDefinition method_P, IlInstructionInjectorAvailableVariables availableInformations)
      {
          var interceptor = new VariableDefinition(method_P.Module.Import(aspect_P.Type));
         Variables.Add(interceptor);
         lifeCycles[aspect_P.LifeCycle].CreateInterceptor(this, interceptor);
         Instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
      }

   }

    public class MethodWeavingBeforeMethodInjector<T> : IIlInjector<T>
        where T : IlInstructionInjectorAvailableVariables
    {
       private readonly MethodDefinition _method;
        private readonly ParametersIlGenerator<T> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private readonly ParametersChecker parametersChecker;
       private AspectBuilder aspectBuilder;
        private NetAspectDefinition aspect;


        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P, ParametersChecker parametersChecker,
                                                 ParametersIlGenerator<T> ilGenerator, NetAspectDefinition aspect)
        {
            _method = method_P;
            interceptorMethod = interceptorMethod_P;
           this.parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
            this.aspect = aspect;
        }

        public void Check(ErrorHandler errorHandler, T availableInformations)
        {
            parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
        }

        public void Inject(List<Instruction> instructions, T availableInformations)
        {
             
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            aspectBuilder.CreateInterceptor(aspect, _method, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}