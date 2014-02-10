using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine
{
    public class InterceptorCaller
    {
        private readonly Collection<Instruction> _instructions;

        private readonly Dictionary<string, Action<ParameterInfo>> forParameters =
            new Dictionary<string, Action<ParameterInfo>>();

        public InterceptorCaller(Collection<Instruction> instructions)
        {
            _instructions = instructions;
        }

        public void AddVariable(string parameterName, VariableDefinition variable, bool updateAllowed)
        {
            forParameters.Add(parameterName, p =>
                {
                    Check(p, updateAllowed, variable.VariableType);
                    if (variable == null)
                       _instructions.Add(Instruction.Create(OpCodes.Ldnull));
                    else
                        _instructions.Add(Instruction.Create(p.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, variable));
                });
        }

        public void AddParameter(string parameterName, ParameterDefinition parameter, bool updateAllowed)
        {
            forParameters.Add(parameterName, p =>
                {
                    Check(p, updateAllowed, parameter.ParameterType);
                    
                        
                });
        }

        private void Check(ParameterInfo parameterInfo, bool updateAllowed, TypeReference variableType)
        {
            if (parameterInfo.ParameterType == typeof(object))
                return;
            if (!IsTypeCompliant(parameterInfo.ParameterType, variableType))
            {
                throw new NotSupportedException("parameter type not supported");
            }
            if (parameterInfo.ParameterType.IsByRef && !updateAllowed)
            {
                throw new NotSupportedException("impossible to ref/out this parameter");
            }
        }

        private static bool IsTypeCompliant(Type parameterType, TypeReference variableType)
        {
            if (parameterType == null)
                return false;
            if (parameterType.FullName.Replace("&", "") == variableType.FullName.Replace("&", ""))
                return true;
            return IsTypeCompliant(parameterType.BaseType, variableType);
        }

        public void AddThis(string parameterName)
        {
            forParameters.Add(parameterName, (p) => _instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));
        }

        public void AddParameters(IEnumerable<ParameterDefinition> parameters)
        {
            foreach (ParameterDefinition parameterDefinition in parameters)
            {
                ParameterDefinition definition = parameterDefinition;
                AddParameter(definition.Name, definition, true);
            }
        }

        //public void Call(VariableDefinition interceptorVariable, MethodInfo method,
        //                 MethodWeavingConfiguration netAspect_P)
        //{
        //    if (method == null)
        //        return;
        //    _instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptorVariable));

        //    foreach (ParameterInfo parameterInfo in method.GetParameters())
        //    {
        //        if (!forParameters.ContainsKey(parameterInfo.Name))
        //            throw new Exception(
        //                string.Format("Parameter {0} not recognized in interceptor {1}.{2} for method {3} in {4}",
        //                              parameterInfo.Name, netAspect_P.Type.Name, method.Name, _methodDefinition.Name,
        //                              _methodDefinition.DeclaringType.Name));
        //        forParameters[parameterInfo.Name](parameterInfo);
        //    }
        //    _instructions.Add(Instruction.Create(OpCodes.Call, _methodDefinition.Module.Import(method)));
        //}
    }
}