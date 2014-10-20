using System.IO;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Method.InterceptorParameters
{
    public static class PdbPossiblity
    {
        public static CommonWeavingInfo AddLineNumberForMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("linenumber")
               .WhichCanNotBeReferenced()
               .WhichPdbPresentForMethod(weavingInfo_P)
               .WhichMustBeOfType<VariablesForMethod, int>()
               .AndInjectThePdbInfoForMethod(s => s.StartLine, weavingInfo_P);
            return weavingInfo_P;
        }
        public static CommonWeavingInfo AddColumnNumberForMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("columnnumber")
               .WhichCanNotBeReferenced()
               .WhichPdbPresentForMethod(weavingInfo_P)
               .WhichMustBeOfType<VariablesForMethod, int>()
               .AndInjectThePdbInfoForMethod(s => s.StartColumn, weavingInfo_P);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddFilePathForMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("filepath")
                                              .WhichCanNotBeReferenced()
                                              .WhichPdbPresentForMethod(weavingInfo_P)
                                              .WhichMustBeOfType<VariablesForMethod, string>()
                                              .AndInjectThePdbInfoForMethod(s => s.Document.Url, weavingInfo_P);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddFileNameForMethod(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("filename")
               .WhichCanNotBeReferenced()
               .WhichPdbPresentForMethod(weavingInfo_P)
               .WhichMustBeOfType<VariablesForMethod, string>()
               .AndInjectThePdbInfoForMethod(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
            return weavingInfo_P;
        }
    }
}