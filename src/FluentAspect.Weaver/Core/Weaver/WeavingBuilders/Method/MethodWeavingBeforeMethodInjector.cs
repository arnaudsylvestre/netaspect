using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
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

      public List<FieldDefinition> Fields = new List<FieldDefinition>();
      public List<VariableDefinition> Variables = new List<VariableDefinition>();
      public List<Instruction> Instructions = new List<Instruction>();
      private readonly MethodDefinition _method;
      private readonly NetAspectDefinition aspect;

      public NetAspectDefinition Aspect
      {
         get { return aspect; }
      }

      public MethodDefinition Method
      {
         get { return _method; }
      }

      public AspectBuilder(NetAspectDefinition aspect_P, MethodDefinition method_P, Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         aspect = aspect_P;
         _method = method_P;
         lifeCycles = lifeCycles_P;
      }

      public void CreateInterceptor()
      {
         var interceptor = new VariableDefinition(_method.Module.Import(aspect.Type));
         Variables.Add(interceptor);
         lifeCycles[aspect.LifeCycle].CreateInterceptor(this, interceptor);
         Instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
      }

   }

    public class MethodWeavingBeforeMethodInjector<T> : IIlInjector<T>
    {
       private readonly MethodDefinition _method;
        private readonly ParametersIlGenerator<T> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private readonly ParametersChecker parametersChecker;
       private AspectBuilder aspectBuilder;


        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P, ParametersChecker parametersChecker,
                                                 ParametersIlGenerator<T> ilGenerator)
        {
            _method = method_P;
            interceptorMethod = interceptorMethod_P;
           this.parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
        }

        public void Check(ErrorHandler errorHandler, T availableInformations)
        {
            parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
        }

        public void Inject(List<Instruction> instructions, T availableInformations)
        {
            
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            aspectBuilder.CreateInterceptor();
           prendre en compte les infos dans aspectBuilder
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}