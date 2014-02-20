using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public class IlInterceptor
    {
        public VariableDefinition Variable;
        public MethodInfo MethodToCall;
    }

   public class IlInjectorAvailableVariables
   {
      private readonly VariableDefinition _result;
      public Collection<Instruction> Instructions = new Collection<Instruction>();
      private MethodDefinition method;
      private VariableDefinition currentMethodInfo;

      public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method)
      {
          _result = result;
          this.method = method;
      }


       public VariableDefinition CurrentMethodInfo { get
      {
         if (currentMethodInfo == null)
         {
            currentMethodInfo = method.CreateVariable<MethodInfo>();

            Instructions.AppendCallToThisGetType(method.Module);
            Instructions.AppendCallToGetMethod(method.Name, method.Module);
            Instructions.AppendSaveResultTo(currentMethodInfo);
         }
         return currentMethodInfo;
      } }

      public VariableDefinition Result
      {
         get { return _result; }
      }
   }
}