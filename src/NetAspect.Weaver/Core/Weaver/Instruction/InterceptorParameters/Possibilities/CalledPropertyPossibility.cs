using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CalledPropertyPossibility
    {
        public static InstructionWeavingInfo AddCalledPropertyInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            InterceptorParametersRulesExtensions.AndInjectTheVariable(interceptorParameterPossibilitiesP.AddPossibleParameter("property")
                                                                   .WhichCanNotBeReferenced()
                                                                   .WhichMustBeOfType<VariablesForInstruction, PropertyInfo>(), instruction => instruction.CalledProperty);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddPropertyValueForInstruction(this InstructionWeavingInfo weavingInfo_P,
                                                                       InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            ParameterDefinition parameter = weavingInfo_P.GetOperandAsMethod().Parameters.Last();
            interceptorParameterPossibilitiesP.AddPossibleParameter("newpropertyvalue")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfTypeOfParameter(parameter)
                                              .AndInjectTheCalledValue(parameter);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddValue(this CommonWeavingInfo weavingInfo_P,
                                                 InterceptorParameterPossibilities<VariablesForMethod>
                                                     interceptorParameterPossibilitiesP)
        {
            ParameterDefinition parameter = weavingInfo_P.Method.Parameters[0];
            interceptorParameterPossibilitiesP.AddPossibleParameter("propertyvalue")
                                              .WhichCanNotBeOut()
                                              .WhichMustBeOfTypeOfParameter(parameter)
                                              .AndInjectTheParameter(parameter);
            return weavingInfo_P;
        }
    }
}