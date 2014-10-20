using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CalledFieldPossibility
    {
        public static InstructionWeavingInfo AddCalledFieldInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("field")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForInstruction, FieldInfo>()
                                              .AndInjectTheCalledFieldInfo(weavingInfo_P);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddFieldValue(this InstructionWeavingInfo weavingInfo_P,
                                                      InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            FieldDefinition field = weavingInfo_P.GetOperandAsField();
            TypeReference parameter = field.FieldType;
            interceptorParameterPossibilitiesP.AddPossibleParameter("fieldvalue")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfTypeOf(parameter)
                                              .AndInjectTheFieldValue(field);
            return weavingInfo_P;
        }
    }
}