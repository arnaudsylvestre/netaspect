﻿using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters
{
    public static class CallerParametersFactory
    {
        public static void AddCaller(this ParametersEngine engine, JoinPoint point)
        {
            engine.AddPossibleParameter("caller",
                                        (info, handler) => Ensure.ParameterType(info, handler, point.Method.DeclaringType,
                                                                                typeof(object)), (info, instructions) => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));
        }


        public static void AddCallerParameters(this ParametersEngine engine, JoinPoint joinPoint)
        {
            foreach (ParameterDefinition parameter_L in joinPoint.Method.Parameters)
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
         
    }
}