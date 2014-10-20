using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{
    public static class MemberKindCheckers
    {

        public static InterceptorParameterPossibility<T> WhereFieldCanNotBeStatic<T>(this InterceptorParameterPossibility<T> possibility, IMemberDefinition member) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsureMethod.IsNotStaticButDefaultValue(parameter, errorListener, member));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhereCurrentMethodCanNotBeStatic<T>(this InterceptorParameterPossibility<T> possibility, CommonWeavingInfo weavingInfo) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsureMethod.IsNotStatic(parameter, errorListener, weavingInfo.Method));
            return possibility;
        }
    }
}