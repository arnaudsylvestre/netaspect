using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class AroundMethodWeaver
   {
       public AroundMethodWeavingModel Model = new AroundMethodWeavingModel();

       public void Check(ErrorHandler errorHandler, VariablesForMethod availableVariables)
      {
          Model.Befores.Check(errorHandler, availableVariables);
          Model.BeforeConstructorBaseCalls.Check(errorHandler, availableVariables);
          Model.Afters.Check(errorHandler, availableVariables);
          Model.OnExceptions.Check(errorHandler, availableVariables);
          Model.OnFinallys.Check(errorHandler, availableVariables);
      }

      public void Inject(List<Instruction> befores, List<Instruction> afters, List<Instruction> onExceptions, List<Instruction> onFinallys, VariablesForMethod availableVariables, List<Instruction> beforeConstructorBaseCall_P)
      {
         Model.BeforeConstructorBaseCalls.Inject(beforeConstructorBaseCall_P, availableVariables);
         Model.Befores.Inject(befores, availableVariables);
         Model.Afters.Inject(afters, availableVariables);
         Model.OnExceptions.Inject(onExceptions, availableVariables);
         Model.OnFinallys.Inject(onFinallys, availableVariables);
      }
   }
}
