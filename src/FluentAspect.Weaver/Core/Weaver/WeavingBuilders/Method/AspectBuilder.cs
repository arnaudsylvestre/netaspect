﻿using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
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
}