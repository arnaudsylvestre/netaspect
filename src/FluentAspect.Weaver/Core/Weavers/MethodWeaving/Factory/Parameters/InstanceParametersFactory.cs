using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public static class InstanceParametersFactory
    {
        public static void AddInstance(this ParametersEngine engine, MethodDefinition methodDefinition)
        {

            engine.AddPossibleParameter("instance",
                                        (p, handler) =>
                                        {
                                            Ensure.NotReferenced(p, handler);
                                            Ensure.OfType(p, handler, typeof(object).FullName, methodDefinition.DeclaringType.FullName);
                                            Ensure.NotStatic(p, handler, methodDefinition);
                                        }, (info, instructions) => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));
        }

        public static void AddParameters(this ParametersEngine engine, VariableDefinition parameters)
        {
            engine.AddPossibleParameter("parameters",
                                        (p, handler) =>
                                        {
                                            Ensure.NotReferenced(p, handler);
                                            Ensure.OfType<object[]>(p, handler);
                                        }, (info, instructions) =>
                                            instructions.Add(Instruction.Create(OpCodes.Ldloc, parameters))
                                            );
        }

        public static void AddMethod(this ParametersEngine engine, VariableDefinition methodVariable)
        {
            engine.AddPossibleParameter("method",
                                        (p, handler) =>
                                        {
                                            Ensure.NotReferenced(p, handler);
                                            Ensure.OfType(p, handler, typeof(MethodInfo).FullName);
                                        }, (info, instructions) =>
                                            instructions.Add(Instruction.Create(OpCodes.Ldloc, methodVariable))
                                            );

        }

        public static void AddResult(this ParametersEngine engine, MethodDefinition methodDefinition, VariableDefinition result)
        {
            engine.AddPossibleParameter("result",
                                        (p, handler) =>
                                        {
                                            Ensure.ResultOfType(p, handler, methodDefinition);
                                        }, (info, instructions) =>
                                            instructions.Add(Instruction.Create(info.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, result))

                                            );

        }

        public static void AddException(this ParametersEngine engine, VariableDefinition exception)
        {

            engine.AddPossibleParameter("exception",
                                        (p, handler) =>
                                        {
                                           Ensure.NotReferenced(p, handler);
                                            Ensure.OfType(p, handler, typeof(Exception).FullName);
                                        }, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, exception)); }

                                            );
        }

        public static void AddParameterValue(this ParametersEngine engine, ParameterDefinition parameter)
        {

           engine.AddPossibleParameter("value",
                                       (p, handler) =>
                                       {
                                          Ensure.NotOut(p, handler);
                                          Ensure.OfType(p, handler, parameter.ParameterType.FullName.Replace("/", "+"));
                                       }, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter)); }

                                           );
        }

        public static void AddParameterNames(this ParametersEngine engine, MethodDefinition methodDefinition, ErrorHandler errorHandler)
        {
            foreach (var parameter in methodDefinition.Parameters)
            {
                try
                {
                    ParameterDefinition parameter1 = parameter;
                    engine.AddPossibleParameter(parameter1.Name,
                                        (p, handler) =>
                                        {
                                            Ensure.OfType(p, handler, parameter1);
                                        }, (p, _instructions) =>
                                            {
                                                var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
                                                if (p.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

                                                }
                                                else if (!p.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(p.ParameterType)));
                                                }
                                                else
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

                                                }
                                                if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                                                    p.ParameterType == typeof(Object))
                                                {
                                                    TypeReference reference = parameter.ParameterType;
                                                    if (reference.IsByReference)
                                                    {
                                                        reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
                                                        _instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

                                                    }
                                                    _instructions.Add(Instruction.Create(OpCodes.Box, reference));
                                                }
                                            }
                                            );
                }
                catch (Exception)
                {
                    errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", parameter.Name));
                }


            
            }
        }
    }
}