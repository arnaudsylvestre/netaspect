using System.Reflection;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Parameters.InterceptorParameters
{
    public static class CurrentParameterPossibility
    {
        public static ParameterWeavingInfo AddParameterValue(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parametervalue")
                                              .WhichCanNotBeOut()
                                              .WhichMustBeOfTypeOfParameter(weavingInfo_P.Parameter)
                                              .AndInjectTheParameter(weavingInfo_P.Parameter);
            return weavingInfo_P;
        }

        public static ParameterWeavingInfo AddParameterName(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parametername")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForMethod, string>()
                                              .AndInjectTheValue(weavingInfo_P.Parameter.Name);
            return weavingInfo_P;
        }

        public static ParameterWeavingInfo AddParameterInfo(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parameter")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForMethod, ParameterInfo>()
                                              .AndInjectTheParameterInfo(weavingInfo_P.Parameter, weavingInfo_P.Method);
            return weavingInfo_P;
        }
    }
}