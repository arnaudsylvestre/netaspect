using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CalledPossibility
   {
       public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP, IMemberDefinition member)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("called")
            .WhichCanNotBeReferenced()
            .WhereFieldCanNotBeStatic(member)
            .WhichMustBeOfTypeOf(member.DeclaringType)
            .AndInjectTheCalledInstance();
         return weavingInfo_P;
      }

       public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
       {
           interceptorParameterPossibilitiesP.AddPossibleParameter("calledparameters")
              .WhichCanNotBeReferenced()
              .WhichMustBeOfType<VariablesForInstruction, object[]>()
              .AndInjectTheVariable(variables => variables.CalledParametersObjects.Definition);
           return weavingInfo_P;
       }

       public static InstructionWeavingInfo AddCalledParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
       {
           foreach (ParameterDefinition parameterDefinition in weavingInfo_P.GetOperandAsMethod().Parameters)
           {
               interceptorParameterPossibilitiesP.AddPossibleParameter("called" + parameterDefinition.Name.ToLower())
                  .WhichCanNotBeReferenced()
                  .WhichMustBeOfTypeOfParameter(parameterDefinition)
                  .AndInjectTheCalledParameter(parameterDefinition);
           }
           return weavingInfo_P;
       }
   }
}
