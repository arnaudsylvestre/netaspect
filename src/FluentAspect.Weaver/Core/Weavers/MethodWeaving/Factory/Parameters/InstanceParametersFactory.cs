using System;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters
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
                                        }, (info, instructions) => Instruction.Create(OpCodes.Ldarg_0));
        }

        public static void AddParameters(this ParametersEngine engine, VariableDefinition parameters)
        {
            engine.AddPossibleParameter("parameters",
                                        (p, handler) =>
                                        {
                                            Ensure.NotReferenced(p, handler);
                                            Ensure.OfType<object[]>(p, handler);
                                        }, (info, instructions) =>
                                            Instruction.Create(OpCodes.Ldloc, parameters)
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
                                            Instruction.Create(OpCodes.Ldloc, methodVariable)
                                            );

        }

        public static void AddResult(this ParametersEngine engine, MethodDefinition methodDefinition)
        {
            engine.AddPossibleParameter("result",
                                        (p, handler) =>
                                        {
                                            Ensure.ResultOfType(p, handler, methodDefinition);
                                        }, (info, instructions) => { throw new NotImplementedException(); }

                                            );

        }

        public static void AddException(this ParametersEngine engine)
        {

            engine.AddPossibleParameter("exception",
                                        (p, handler) =>
                                        {
                                            Ensure.OfType(p, handler, typeof(Exception).FullName);
                                        }, (info, instructions) => { throw new NotImplementedException(); }

                                            );
        }

        public static void AddParameterNames(this ParametersEngine engine, MethodDefinition methodDefinition, ErrorHandler errorHandler)
        {
            foreach (var parameter in methodDefinition.Parameters)
            {
                try
                {
                    ParameterDefinition parameter1 = parameter;
                    engine.AddPossibleParameter("parameters",
                                        (p, handler) =>
                                        {
                                            Ensure.OfType(p, handler, parameter1);
                                        }, (info, instructions) =>
                                            Instruction.Create(OpCodes.Ldloc, methodDefinition.Parameters.First(p => p.Name == info.Name))
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