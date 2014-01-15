using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
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
                    FillParameters(instructionP_P, parameters, instructions, variableParameters, reference);


                    foreach (ParameterInfo parameterInfo_L in afterCallMethod.GetParameters())
                    {
                        parameters[parameterInfo_L.Name.ToLower()](parameterInfo_L);
                    }

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        private void FillParameters(SequencePoint instructionP_P, Dictionary<string, Action<ParameterInfo>> parameters, List<Instruction> instructions, List<KeyValue> variablesForParameters, MethodReference reference_P)
        {
           foreach (var parameterDefinition_L in reference_P.Parameters.Reverse())
           {
              ParameterDefinition definition_L = parameterDefinition_L;
               var variable =
                   (from v in variablesForParameters where v.ParameterName == definition_L.Name select v.Variable).First
                       ();
               parameters.Add((parameterDefinition_L.Name + "Called").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldloc, variable)));
           }

           parameters.Add("linenumber", p => instructions.Add(Create(instructionP_P, i => i.StartLine)));
            parameters.Add("columnnumber", p => instructions.Add(Create(instructionP_P, i => i.StartColumn)));
            parameters.Add("filename", p => instructions.Add(Create(instructionP_P, i => Path.GetFileName(i.Document.Url))));
            parameters.Add("filepath", p => instructions.Add(Create(instructionP_P, i => i.Document.Url)));
            parameters.Add("caller", p => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));

            foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
            {
                ParameterDefinition parameter1_L = parameter_L;
                parameters.Add((parameter1_L.Name + "Caller").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L)));
            }

        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, int> provider)
        {
            return Instruction.Create(OpCodes.Ldc_I4,
                                      instructionP_P == null
                                          ? 0
                                          : provider(instructionP_P));
        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, string> provider)
        {
            return Instruction.Create(OpCodes.Ldstr,
                                      instructionP_P == null
                                          ? null
                                          : provider(instructionP_P));
        }

        
    }
}