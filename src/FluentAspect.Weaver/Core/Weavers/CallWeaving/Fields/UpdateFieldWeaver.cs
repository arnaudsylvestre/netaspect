using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Fields
{

    public class UpdateFieldWeaver : ICallWeavingProvider
    {
        private readonly FieldToWeave _toWeave;
        private ParametersEngine parametersEngine;
        VariableDefinition value;

        public UpdateFieldWeaver(FieldToWeave toWeave)
        {
            _toWeave = toWeave;
            value = toWeave.JoinPoint.Method.CreateVariable(toWeave.Field.FieldType);
            parametersEngine = ParametersEngineFactory.CreateForUpdateField(_toWeave.JoinPoint, value);
        }

        public void AddBefore(List<Instruction> beforeInstructions)
        {
            beforeInstructions.Add(Instruction.Create(OpCodes.Stloc, value));

            foreach (var interceptorType in _toWeave.Interceptors)
            {
                var method = interceptorType.BeforeUpdateFieldValue.Method;
                if (method == null) continue;
                parametersEngine.Fill(method.GetParameters(), beforeInstructions);
                beforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                                   _toWeave.JoinPoint.Method.Module.Import(
                                                                       method)));
            }
            beforeInstructions.Add(Instruction.Create(OpCodes.Ldloc, value));
        }

        public void AddAfter(List<Instruction> instructions)
        {
            foreach (var interceptorType in _toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.AfterUpdateFieldValue.Method;
                if (afterCallMethod == null) continue;
                parametersEngine.Fill(afterCallMethod.GetParameters(), instructions);
                instructions.Add(Instruction.Create(OpCodes.Call, _toWeave.JoinPoint.Method.Module.Import(afterCallMethod)));
            }
        }

        public void Check(ErrorHandler error)
        {
            foreach (var netAspectAttribute in _toWeave.Interceptors)
            {
                parametersEngine.Check(netAspectAttribute.BeforeUpdateFieldValue.GetParameters(), error);
                parametersEngine.Check(netAspectAttribute.AfterUpdateFieldValue.GetParameters(), error);
            }
        }
    }
}