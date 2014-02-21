using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public class IlInjectorAvailableVariables
   {
       private readonly VariableDefinition _result;
       public Collection<Instruction> Instructions = new Collection<Instruction>();
       public Collection<Instruction> ExceptionManagementInstructions = new Collection<Instruction>();
      private MethodDefinition method;
      private VariableDefinition currentMethodInfo;
        private VariableDefinition _parameters;
        private VariableDefinition _exception;

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

        public VariableDefinition Parameters
        {
            get { if (_parameters == null)
            {
                _parameters = method.CreateVariable<object[]>();

                new Method(method).FillArgsArrayFromParameters(Instructions, _parameters);
            }
                return _parameters;
            }
        }

        public VariableDefinition Exception
        {
            get { if (_exception == null)
            {
                _exception = method.CreateVariable<Exception>();
                ExceptionManagementInstructions.Add(Instruction.Create(OpCodes.Stloc, _exception));
            }
                return _exception;
            }
        }
   }
}