using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class CallMethodWeaver : IWeaveable
    {
        private CallToWeave toWeave;
        private ParametersEngine engine;

        public CallMethodWeaver(ParametersEngine engine, CallToWeave toWeave)
        {
            this.engine = engine;
            this.toWeave = toWeave;
        }


        public void Weave(ErrorHandler errorP_P)
        {
            var reference = toWeave.Instruction.Operand as MethodReference;
            toWeave.MethodToWeave.Body.InitLocals = true;
            

        }

        public void Check(ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in toWeave.Interceptors)
            {
                engine.Check(netAspectAttribute.BeforeInterceptor.GetParameters(), errorHandler);
                engine.Check(netAspectAttribute.AfterInterceptor.GetParameters(), errorHandler);
            }
        }

        public class KeyValue
        {
            public VariableDefinition Variable;
            public string ParameterName;
        }

        public bool CanWeave()
        {
            return true;
        }

        private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module, SequencePoint instructionP_P, List<KeyValue> variableParameters, MethodReference reference)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.AfterInterceptor.Method;
                if (afterCallMethod != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    engine.Fill(afterCallMethod.GetParameters(), instructions);

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        
    }
}