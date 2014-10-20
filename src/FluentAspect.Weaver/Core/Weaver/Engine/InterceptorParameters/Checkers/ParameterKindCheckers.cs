using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers.Ensures;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{
    public static class ParameterKindCheckers
    {
        public static InterceptorParameterPossibility<T> WhichCanNotBeReferenced<T>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
        {
            possibility.Checkers.Add(EnsureParameter.IsNotReferenced);
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhichCanNotBeOut<T>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
        {
            possibility.Checkers.Add(EnsureParameter.IsNotOut);
            return possibility;
        }
        
    }
}