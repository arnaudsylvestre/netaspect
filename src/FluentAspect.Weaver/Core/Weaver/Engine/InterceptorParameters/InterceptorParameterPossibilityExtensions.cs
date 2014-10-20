using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters
{
    public static class InterceptorParameterPossibilityExtensions
    {


        public static void AddChecker<T>(this InterceptorParameterPossibility<T> checkers, IChecker checker) where T : VariablesForMethod
        {
            checkers.Checkers.Add(checker.Check);
        }

        public static void Check<T>(this InterceptorParameterPossibility<T> possibility, ParameterInfo parameter, ErrorHandler errorListener) where T : VariablesForMethod
        {
            foreach (var checker in possibility.Checkers)
            {
                checker(parameter, errorListener);
            }
        }


        public static void GenerateIl<T>(this InterceptorParameterPossibility<T> possibility, ParameterInfo parameterInfo, List<Mono.Cecil.Cil.Instruction> instructions, T info) where T : VariablesForMethod
        {
            foreach (var generator in possibility.Generators)
            {
                generator(parameterInfo, instructions, info);
            }
        }
    }
}