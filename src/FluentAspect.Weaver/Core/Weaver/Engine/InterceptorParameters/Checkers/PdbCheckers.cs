using System.Linq;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{
    public static class PdbCheckers
    {
        public static InterceptorParameterPossibility<T> WhichPdbPresent<T>(this InterceptorParameterPossibility<T> possibility, InstructionWeavingInfo info) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsurePdb.IsPresent(info.Instruction, errorListener, parameter));
            return possibility;
        }

        public static InterceptorParameterPossibility<T> WhichPdbPresentForMethod<T>(this InterceptorParameterPossibility<T> possibility, CommonWeavingInfo info) where T : VariablesForMethod
        {
            possibility.Checkers.Add((parameter, errorListener) => EnsurePdb.IsPresent(info.Method.Body.Instructions.First(), errorListener, parameter));
            return possibility;
        }
    }
}