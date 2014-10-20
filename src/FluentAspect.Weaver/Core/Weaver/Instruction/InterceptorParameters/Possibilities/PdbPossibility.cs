using System.IO;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class PdbPossibility
    {

        public static InstructionWeavingInfo AddColumnNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("columnnumber")
                                              .WhichCanNotBeReferenced()
                                              .WhichPdbPresent(weavingInfo_P)
                                              .WhichMustBeOfType<VariablesForInstruction, int>()
                                              .AndInjectThePdbInfo(s => s.StartColumn, weavingInfo_P);
            return weavingInfo_P;
        }

        public static InstructionWeavingInfo AddLineNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("linenumber")
                                              .WhichCanNotBeReferenced()
                                              .WhichPdbPresent(weavingInfo_P)
                                              .WhichMustBeOfType<VariablesForInstruction, int>()
                                              .AndInjectThePdbInfo(s => s.StartLine, weavingInfo_P);
            return weavingInfo_P;
        }

        public static InstructionWeavingInfo AddFilePath(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("filepath")
               .WhichCanNotBeReferenced()
               .WhichPdbPresent(weavingInfo_P)
               .WhichMustBeOfType<VariablesForInstruction, string>()
               .AndInjectThePdbInfo(s => s.Document.Url, weavingInfo_P);
            return weavingInfo_P;
        }



        public static InstructionWeavingInfo AddFileName(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("filename")
               .WhichCanNotBeReferenced()
               .WhichPdbPresent(weavingInfo_P)
               .WhichMustBeOfType<VariablesForInstruction, string>()
               .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
            return weavingInfo_P;
        }
    }
}