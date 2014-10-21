using System;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Method.InterceptorParameters
{
    public static class MethodPossibility
    {
        public static CommonWeavingInfo AddException(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("exception")
               .WhichCanNotBeReferenced()
               .WhichMustBeOfType<VariablesForMethod, Exception>()
               .AndInjectTheVariable(variables => variables.Exception.Definition);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddResult(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("result")
               .WhereParameterTypeIsSameAsMethodResult(weavingInfo_P)
               .AndInjectTheVariable(variables => variables.Result.Definition);
            return weavingInfo_P;
        }
    }
}