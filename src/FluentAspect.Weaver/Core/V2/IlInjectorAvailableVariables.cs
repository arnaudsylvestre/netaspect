﻿using System;
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
       private MethodDefinition method;
       private VariableDefinition currentMethodInfo;
       private VariableDefinition currentPropertyInfo;
        private VariableDefinition _parameters;
        private VariableDefinition _exception;

        public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method)
      {
          _result = result;
          this.method = method;
      }


       public VariableDefinition CurrentMethodBase { get
      {
         if (currentMethodInfo == null)
         {
            currentMethodInfo = method.CreateVariable<MethodBase>();

            Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(typeof(MethodBase).GetMethod("GetCurrentMethod", new Type[] {  }))));
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
            }
                return _exception;
            }
        }

        public VariableDefinition CurrentPropertyInfo
        {
            get
            {
                if (currentPropertyInfo == null)
                {
                    currentPropertyInfo = method.CreateVariable<PropertyInfo>();

                    Instructions.AppendCallToThisGetType(method.Module);
                    Instructions.AppendCallToGetProperty(method.Name.Replace("get_", "").Replace("set_", ""), method.Module);
                    Instructions.AppendSaveResultTo(currentPropertyInfo);
                }
                return currentPropertyInfo;
            }
        }
   }
}