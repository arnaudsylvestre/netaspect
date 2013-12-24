﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Weavers
{
    public class InterceptorCaller
    {
        private readonly MethodDefinition _methodDefinition;

        private readonly Dictionary<string, Action<ParameterInfo>> forParameters =
            new Dictionary<string, Action<ParameterInfo>>();

        private readonly ILProcessor il;

        public InterceptorCaller(ILProcessor il, MethodDefinition methodDefinition)
        {
            this.il = il;
            _methodDefinition = methodDefinition;
        }

        public void AddVariable(string parameterName, VariableDefinition variable, bool updateAllowed)
        {
            forParameters.Add(parameterName, p =>
                {
                    Check(p, updateAllowed, variable.VariableType);
                    if (variable == null)
                        il.Emit(OpCodes.Ldnull);
                    else
                        il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, variable);
                });
        }

        public void AddParameter(string parameterName, ParameterDefinition parameter, bool updateAllowed)
        {
            forParameters.Add(parameterName, p =>
                {
                    Check(p, updateAllowed, parameter.ParameterType);
                    il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldarga : OpCodes.Ldarg, parameter);
                });
        }

        private void Check(ParameterInfo parameterInfo, bool updateAllowed, TypeReference variableType)
        {
            if (!IsTypeCompliant(parameterInfo.ParameterType, variableType))
            {
                throw new NotSupportedException("parameter type not supported");
            }
            if (parameterInfo.ParameterType.IsByRef && !updateAllowed)
            {
                throw new NotSupportedException("impossible to ref this parameter");
            }
        }

        private static bool IsTypeCompliant(Type parameterType, TypeReference variableType)
        {
            if (parameterType == null)
                return false;
            if (parameterType.FullName.Replace("&", "") == variableType.FullName)
                return true;
            return IsTypeCompliant(parameterType.BaseType, variableType);
        }

        public void AddThis(string parameterName)
        {
            forParameters.Add(parameterName, (p) => il.Emit(OpCodes.Ldarg_0));
        }

        public void AddParameters(IEnumerable<ParameterDefinition> parameters)
        {
            foreach (ParameterDefinition parameterDefinition in parameters)
            {
                ParameterDefinition definition = parameterDefinition;
                AddParameter(definition.Name, definition, true);
            }
        }

        public void Call(VariableDefinition interceptorVariable, string methodToCall, Type iType)
        {
            MethodInfo method = iType.GetMethod(methodToCall);
            if (method == null)
                return;
            il.Emit(OpCodes.Ldloc, interceptorVariable);

            foreach (ParameterInfo parameterInfo in method.GetParameters())
            {
                if (!forParameters.ContainsKey(parameterInfo.Name))
                    throw new Exception(
                        string.Format("Parameter {0} not recognized in interceptor {1}.{2} for method {3} in {4}",
                                      parameterInfo.Name, iType.Name, method.Name, _methodDefinition.Name,
                                      _methodDefinition.DeclaringType.Name));
                forParameters[parameterInfo.Name](parameterInfo);
            }
            il.Emit(OpCodes.Call, _methodDefinition.Module.Import(method));
        }
    }
}