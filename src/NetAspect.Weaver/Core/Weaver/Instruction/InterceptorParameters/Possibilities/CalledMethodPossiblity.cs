using System.Reflection;
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
        public static InstructionWeavingInfo AddCallFieldValue(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("fieldvalue")
                                              .WhereParameterTypeIsSameAsFieldTypeAndNotReferenced(weavingInfo_P)
                                              .AndInjectTheVariable(variables => variables.ResultForInstruction.Definition);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledConstructorInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("constructor")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForInstruction, MethodBase>()
                                              .AndInjectTheVariable(variables => variables.CalledConstructor.Definition);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledMethod(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("method")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForInstruction, MethodBase>()
                                              .AndInjectTheVariable(variables => variables.CalledMethod.Definition);
            return weavingInfo_P;
        }
    }
}