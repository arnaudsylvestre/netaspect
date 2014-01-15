using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public interface ICallWeavingProvider
    {
        void AddBefore(List<Instruction> beforeInstructions);
        void AddAfter(List<Instruction> beforeInstructions);
    }

    public class CallMethodWeavingProvider : ICallWeavingProvider
    {
        private List<CallMethodWeaver.KeyValue> variablesForParameters = new List<CallMethodWeaver.KeyValue>();
        private MethodReference reference;
        private readonly CallToWeave _toWeave;
        private ParametersEngine parametersEngine;

        public CallMethodWeavingProvider(MethodReference reference, CallToWeave toWeave, ParametersEngine parametersEngine)
        {
            this.reference = reference;
            _toWeave = toWeave;
            this.parametersEngine = parametersEngine;
        }

        public void AddBefore(List<Instruction> beforeInstructions)
        {
            Prepare(beforeInstructions);
        }

        private void Prepare(List<Instruction> instructions)
        {
            foreach (var parameterDefinition_L in reference.Parameters.Reverse())
            {
                var variableDefinition_L = _toWeave.MethodToWeave.CreateVariable(parameterDefinition_L.ParameterType);
                instructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition_L));
                variablesForParameters.Add(new CallMethodWeaver.KeyValue()
                {
                    Variable = variableDefinition_L,
                    ParameterName = parameterDefinition_L.Name,
                });
            }
        }

        private IEnumerable<Instruction> CreateBeforeInstructions(SequencePoint pointL, List<CallMethodWeaver.KeyValue> variableParameters)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in _toWeave.Interceptors)
            {
                if (interceptorType.BeforeInterceptor.Method != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    parametersEngine.Fill();
                    FillParameters(pointL, parameters, instructions, variableParameters, reference);
                    instructions.Add(Instruction.Create(OpCodes.Call,
                                                        _toWeave.MethodToWeave.Module.Import(
                                                            interceptorType.BeforeInterceptor
                                                                           .Method)));
                }
            }
            return instructions;
        }

        public void AddAfter(List<Instruction> beforeInstructions)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AroundInstructionWeaver
    {
        private MethodPoint point;

        public AroundInstructionWeaver(MethodPoint point)
        {
            this.point = point;
        }

        public void Weave(ICallWeavingProvider provider)
        {
            SequencePoint point_L = point.Instruction.GetLastSequencePoint();

            var instructions = new List<Instruction>();
            var variablesForParameters = new List<CallMethodWeaver.KeyValue>();
            provider.Prepare(instructions);

            var beforeInstructions = new List<Instruction>();
            provider.AddBefore(beforeInstructions);
            // CreateBeforeInstructions(toWeave.MethodToWeave.Module, point_L, variablesForParameters, reference)
            instructions.AddRange(beforeInstructions);

            foreach (var variablesForParameter in ((IEnumerable<CallMethodWeaver.KeyValue>)variablesForParameters).Reverse())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, variablesForParameter.Variable));
            }

            var afterInstructions = CreateAfterInstructions(toWeave.MethodToWeave.Module, point_L, variablesForParameters, reference).ToList();
            toWeave.MethodToWeave.InsertAfter(toWeave.Instruction, afterInstructions);
            toWeave.MethodToWeave.InsertBefore(toWeave.Instruction, instructions);
        }
    }
}