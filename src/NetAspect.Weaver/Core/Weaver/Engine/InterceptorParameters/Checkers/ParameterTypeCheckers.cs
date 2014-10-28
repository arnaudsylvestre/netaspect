using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers.Ensures;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{
    public static class ParameterTypeCheckers
    {
        public static InterceptorParameterPossibility<T> WhereParameterTypeIsSameAsMethodResult<T>(this InterceptorParameterPossibility<T> possibility, CommonWeavingInfo info) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsureResult.OfType(parameter, errorListener, info.Method));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhereParameterTypeIsSameAsMethodResultAndNotReferenced<T>(this InterceptorParameterPossibility<T> possibility, InstructionWeavingInfo info) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsureResult.OfTypeNotReferenced(parameter, errorListener, info.GetOperandAsMethod()));
            return possibility;
        }
        public static InterceptorParameterPossibility<T> WhereParameterTypeIsSameAsFieldTypeAndNotReferenced<T>(this InterceptorParameterPossibility<T> possibility, InstructionWeavingInfo info) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsureResult.OfTypeNotReferenced(parameter, errorListener, info.GetOperandAsField()));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhichMustBeOfType<T, T1>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
        {
            return possibility.WhichMustBeOfTypeOf(typeof(T1).FullName);
        }

        public static InterceptorParameterPossibility<T> WhichMustBeOfTypeOfParameter<T>(this InterceptorParameterPossibility<T> possibility, ParameterDefinition parameterDefinition) where T : VariablesForMethod
        {
            possibility.Checkers.Add((info, handler) => EnsureParameter.IsOfType(info, handler, parameterDefinition));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhichMustBeOfTypeOf<T>(this InterceptorParameterPossibility<T> possibility, TypeReference type) where T : VariablesForMethod
        {
            WhichMustBeOfTypeOf(possibility, type.FullName);
            return possibility;
        }

        private static InterceptorParameterPossibility<T> WhichMustBeOfTypeOf<T>(this InterceptorParameterPossibility<T> possibility, string fullName) where T : VariablesForMethod
        {
            possibility.AddChecker(new ParameterTypeChecker(fullName, null));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> OrOfCurrentMethodDeclaringType<T>(this InterceptorParameterPossibility<T> possibility, CommonWeavingInfo weavingInfo) where T : VariablesForMethod
        {
            return WhichMustBeOfTypeOf(possibility, weavingInfo.Method.DeclaringType);
        }

    }
}