using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public static class InterceptorInfoExtensions
    {
        public static FieldDefinition GetOperandAsField(this InterceptorInfo interceptor)
        {
            return (interceptor.Instruction.Operand as FieldReference).Resolve();
        }

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AddPossibleParameter(this InterceptorInfo interceptor,
                                                                                                                        string parameterName)
        {
            var myGenerator = new MyGenerator<IlInjectorAvailableVariablesForInstruction>();
            var checker = new MyInterceptorParameterChecker();
            interceptor.Generator.Add(parameterName, myGenerator, checker);
            return new InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction>(myGenerator, checker, interceptor);
        }

        public class MyInterceptorParameterChecker : IInterceptorParameterChecker
        {
            public List<Action<ParameterInfo, ErrorHandler>> Checkers = new List<Action<ParameterInfo, ErrorHandler>>();

            public void Check(ParameterInfo parameter, ErrorHandler errorListener)
            {
                foreach (var checker in Checkers)
                {
                    checker(parameter, errorListener);
                }
            }
        }


        public class MyGenerator<T> : IInterceptorParameterIlGenerator<T>
        {
            public List<Action<ParameterInfo, List<Instruction>, T>> Generators = new List<Action<ParameterInfo, List<Instruction>, T>>();

            public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
            {
                foreach (var generator in Generators)
                {
                    generator(parameterInfo, instructions, info);
                }
            }
        }
    }
}