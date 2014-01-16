using System;
using System.Collections.Generic;
using System.IO;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory
{
    public static class CallWeavingFactory
    {
        public static CallMethodWeaver CreateCallMethodWeaver(MethodPoint point, IEnumerable<CallWeavingConfiguration> interceptorTypes)
         {
            var toWeave = new CallToWeave
                {
                    MethodToWeave = point.Method, Instruction = point.Instruction, Interceptors = interceptorTypes
                };
            var engine = new ParametersEngine();
            AddLineNumber(point, engine, toWeave.Instruction);
            AddColumnNumber(point, engine, toWeave.Instruction);
            AddFilename(point, engine, toWeave.Instruction);
             AddFilepath(engine, toWeave, toWeave.Instruction);
             AddCaller(engine, toWeave);
            AddCallerParameters(engine, toWeave);
            AddCalledParameters(engine, toWeave, toWeave.Instruction.Operand as MethodReference);

             return new CallMethodWeaver(engine, toWeave);
         }

        private static void AddCalledParameters(ParametersEngine engine, CallToWeave toWeave, MethodReference reference_P)
        {
            foreach (var parameterDefinition_L in reference_P.Parameters.Reverse())
            {
                ParameterDefinition definition_L = parameterDefinition_L;
                var variable =
                    (from v in variablesForParameters where v.ParameterName == definition_L.Name select v.Variable).First
                        ();
                parameters.Add((parameterDefinition_L.Name + "Called").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldloc, variable)));
            }
        }

        private static void AddCallerParameters(ParametersEngine engine, CallToWeave toWeave)
        {
            foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
            {
                ParameterDefinition parameter1_L = parameter_L;
                engine.AddPossibleParameter((parameter1_L.Name + "Caller").ToLower(), (info, handler) =>
                    {
                        Ensure.ParameterType(info, handler, parameter1_L.ParameterType, null);
                    }, (info, instructions) =>
                        {
                            instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L));
                        });
            }
        }

        private static void AddCaller(ParametersEngine engine, CallToWeave toWeave)
        {
            engine.AddPossibleParameter("caller",
                                        (info, handler) => Ensure.ParameterType(info, handler, toWeave.MethodToWeave.DeclaringType,
                                                                                typeof (object)), (info, instructions) => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));
        }

        private static void AddFilepath(ParametersEngine engine, CallToWeave toWeave, Instruction instruction)
        {
            engine.AddPossibleParameter("filepath", (info, handler) =>
                {
                    Ensure.SequencePoint(toWeave.Instruction, handler, info);
                    Ensure.ParameterType<string>(info, handler);
                }, 
                (info, instructions) => instructions.Add(Create(instruction.GetLastSequencePoint(), i => i.Document.Url)));
        }

        private static void AddFilename(MethodPoint point, ParametersEngine engine, Instruction instruction)
        {
            engine.AddPossibleParameter("filename", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<string>(info, handler);
                },
                (info, instructions) => instructions.Add(Create(instruction.GetLastSequencePoint(), i => Path.GetFileName(i.Document.Url))));
        }

        private static void AddColumnNumber(MethodPoint point, ParametersEngine engine, Instruction instruction)
        {
            engine.AddPossibleParameter("columnnumber", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<int>(info, handler);
                },
                (info, instructions) => instructions.Add(Create(instruction.GetLastSequencePoint(), i => i.StartColumn)));
        }

        
       

        

        private static void AddLineNumber(MethodPoint point, ParametersEngine engine, Instruction instruction)
        {

            engine.AddPossibleParameter("linenumber", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<int>(info, handler);
                },
                (info, instructions) => instructions.Add(Create(instruction.GetLastSequencePoint(), i => i.StartLine)));
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