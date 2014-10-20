using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CalledMethodPossiblity
    {

        public static InstructionWeavingInfo AddResult(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("result")
                                              .WhereParameterTypeIsSameAsMethodResultAndNotReferenced(weavingInfo_P)
                                              .AndInjectTheVariable(variables => variables.ResultForInstruction.Definition);
            return weavingInfo_P;
        }
    }
}