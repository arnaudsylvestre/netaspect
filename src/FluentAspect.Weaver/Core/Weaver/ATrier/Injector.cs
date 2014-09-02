using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public interface IWevingPreconditionInjector
    {
        void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations, MethodInfo interceptorMethod_P, MethodDefinition method_P);
    }

    public class OverrideWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations, MethodInfo interceptorMethod_P, MethodDefinition method_P)
        {
           var instruction_L = availableInformations.Instruction;
           if (!instruction_L.IsACallInstruction())
              return;
           var calledMethod = instruction_L.GetCalledMethod();
           if (calledMethod.IsVirtual)
           {
               precondition.Add(Instruction.Create(OpCodes.Ldstr, calledMethod.DeclaringType.FullName.Replace('/', '+')));
               precondition.AppendCallToTargetGetType(method_P.Module, availableInformations.Called);
               precondition.AppendCallToGetMethod(calledMethod.Name, method_P.Module);
               precondition.Add(Instruction.Create(OpCodes.Callvirt, method_P.Module.Import(typeof(MemberInfo).GetMethod("get_DeclaringType"))));
               precondition.Add(Instruction.Create(OpCodes.Callvirt, method_P.Module.Import(typeof(Type).GetMethod("get_FullName"))));
               precondition.Add(Instruction.Create(OpCodes.Call, method_P.Module.Import(typeof(string).GetMethod("op_Equality"))));
               
           }
        }
    }

    public class NoWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations, MethodInfo interceptorMethod_P, MethodDefinition method_P)
        {
            
        }
    }

   public class Injector : IIlInjector
   {
      private readonly MethodDefinition _method;
       private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;
       private IWevingPreconditionInjector weavingPreconditionInjector;


       public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
           interceptorParameterConfigurations = interceptorParameterConfigurations_P;
           this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public void Check(ErrorHandler errorHandler)
      {
          if (interceptorMethod.ReturnType != typeof(void))
              errorHandler.OnError(ErrorCode.InterceptorMustBeVoid, FileLocation.None, interceptorMethod.Name, interceptorMethod.DeclaringType.FullName);
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
          Instruction end = Instruction.Create(OpCodes.Nop);
          var precondition = new List<Instruction>();
          weavingPreconditionInjector.Inject(precondition, availableInformations, interceptorMethod, _method);
          if (precondition.Any())
          {
              instructions.AddRange(precondition);
              instructions.Add(Instruction.Create(OpCodes.Brfalse, end));
          }
          InterceptorVariable doit etre multiple
          instructions.Add(Instruction.Create(OpCodes.Ldloc, availableInformations.InterceptorVariable));
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
          instructions.Add(end);
      }
   }
}
