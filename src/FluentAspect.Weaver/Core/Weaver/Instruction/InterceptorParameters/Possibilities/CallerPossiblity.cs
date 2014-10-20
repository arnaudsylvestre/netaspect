using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CallerPossiblity
    {
        public static CommonWeavingInfo AddCallerParameterNames(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return CurrentInstancePossibility.AddParameterNames(weavingInfo_P, interceptorParameterPossibilitiesP, () => "caller{0}");
        }

        public static CommonWeavingInfo AddCallerParameters(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return CurrentInstancePossibility.AddCurrentParameters(weavingInfo_P, interceptorParameterPossibilitiesP, "callerparameters");
        }


        public static CommonWeavingInfo AddCaller(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCurrentInstance(interceptorParameterPossibilitiesP, "caller");
        }

        public static CommonWeavingInfo AddCallerMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddMethod(interceptorParameterPossibilitiesP, "callermethod");
        }
    }
}