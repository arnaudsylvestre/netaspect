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
       public MethodWeavingAspectInstance AspectInstance = new MethodWeavingAspectInstance();

       public void Check(ErrorHandler errorHandler, VariablesForMethod availableVariables)
      {
          AspectInstance.Befores.Check(errorHandler, availableVariables);
          AspectInstance.BeforeConstructorBaseCalls.Check(errorHandler, availableVariables);
          AspectInstance.Afters.Check(errorHandler, availableVariables);
          AspectInstance.OnExceptions.Check(errorHandler, availableVariables);
          AspectInstance.OnFinallys.Check(errorHandler, availableVariables);
      }

      public void Inject(List<Instruction> befores, List<Instruction> afters, List<Instruction> onExceptions, List<Instruction> onFinallys, VariablesForMethod availableVariables, List<Instruction> beforeConstructorBaseCall_P)
      {
         AspectInstance.BeforeConstructorBaseCalls.Inject(beforeConstructorBaseCall_P, availableVariables);
         AspectInstance.Befores.Inject(befores, availableVariables);
         AspectInstance.Afters.Inject(afters, availableVariables);
         AspectInstance.OnExceptions.Inject(onExceptions, availableVariables);
         AspectInstance.OnFinallys.Inject(onFinallys, availableVariables);
      }
   }
}
