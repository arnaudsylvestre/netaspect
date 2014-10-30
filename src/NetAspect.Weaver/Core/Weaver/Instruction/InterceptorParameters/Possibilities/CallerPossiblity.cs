using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CallerPossiblity
    {
        public static CommonWeavingInfo AddCallerParameters(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCurrentParameters(interceptorParameterPossibilitiesP, "callerparameters");
        }


        public static CommonWeavingInfo AddCaller(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCurrentInstance(interceptorParameterPossibilitiesP, "callerinstance");
        }

        public static CommonWeavingInfo AddCallerMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddMethodBase(interceptorParameterPossibilitiesP, "callermethod");
        }
    }
}