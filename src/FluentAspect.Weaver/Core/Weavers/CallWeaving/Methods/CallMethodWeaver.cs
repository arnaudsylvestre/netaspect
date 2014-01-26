using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Methods
{
    public class CallMethodWeaver : ICallWeavingProvider
    {
        private Dictionary<string, VariableDefinition> variablesForParameters = new Dictionary<string, VariableDefinition>();

        private readonly MethodCallToWeave _toWeave;
        private ParametersEngine parametersEngine;

        public CallMethodWeaver(MethodCallToWeave toWeave)
        {
            _toWeave = toWeave;
            parametersEngine = ParametersEngineFactory.CreateForMethodCall(_toWeave.JoinPoint, variablesForParameters);
        }

        public void AddBefore(List<Instruction> beforeInstructions)
        {
            PrepareCalledParameters(beforeInstructions);
            CreateBeforeInstructions(beforeInstructions);
            ReloadParametersForCalledMethod(beforeInstructions);
        }

        private void ReloadParametersForCalledMethod(List<Instruction> instructions)
        {
            foreach (var parameter in _toWeave.CalledMethod.Parameters.Reverse())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, variablesForParameters[parameter.Name]));
            }
        }

        private void PrepareCalledParameters(List<Instruction> beforeInstructions)
        {
            foreach (var parameterDefinition_L in _toWeave.CalledMethod.Parameters.Reverse())
            {
                var variableDefinition_L = _toWeave.JoinPoint.Method.CreateVariable(parameterDefinition_L.ParameterType);
                beforeInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition_L));
                variablesForParameters.Add(parameterDefinition_L.Name, variableDefinition_L);
            }
        }

        private void CreateBeforeInstructions(List<Instruction> instructions)
        {
            foreach (var interceptorType in _toWeave.Interceptors)
            {
                if (interceptorType.Before.Method == null) continue;
                parametersEngine.Fill(interceptorType.Before.Method.GetParameters(), instructions);
                instructions.Add(Instruction.Create(OpCodes.Call,
                                                    _toWeave.JoinPoint.Method.Module.Import(
                                                        interceptorType.Before
                                                                       .Method)));
            }
        }

        public void AddAfter(List<Instruction> instructions)
        {
            foreach (var interceptorType in _toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.After.Method;
                if (afterCallMethod == null) continue;
                parametersEngine.Fill(afterCallMethod.GetParameters(), instructions);
                instructions.Add(Instruction.Create(OpCodes.Call, _toWeave.JoinPoint.Method.Module.Import(afterCallMethod)));
            }
        }

        public void Check(ErrorHandler error)
        {
            foreach (var netAspectAttribute in _toWeave.Interceptors)
            {
                parametersEngine.Check(netAspectAttribute.Before.GetParameters(), error);
                parametersEngine.Check(netAspectAttribute.After.GetParameters(), error);
            }
        }
    }
}