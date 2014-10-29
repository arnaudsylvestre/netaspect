using System.Reflection;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CurrentMemberPossibility
    {
        public static CommonWeavingInfo AddMethod<T>(this CommonWeavingInfo weavingInfo_P,
                                                     InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
                                                     string parameterName)
            where T : VariablesForMethod
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<T, MethodInfo>()
                                              .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static CommonWeavingInfo AddMethodBase<T>(this CommonWeavingInfo weavingInfo_P,
                                                     InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
                                                     string parameterName)
            where T : VariablesForMethod
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<T, MethodBase>()
                                              .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static CommonWeavingInfo AddConstructor<T>(this CommonWeavingInfo weavingInfo_P,
                                                     InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
                                                     string parameterName)
            where T : VariablesForMethod
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<T, ConstructorInfo>()
                                              .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddProperty(this CommonWeavingInfo weavingInfo_P,
                                                    InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("property")
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<VariablesForMethod, PropertyInfo>()
                                              .AndInjectTheCurrentProperty();
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddMethod(this CommonWeavingInfo weavingInfo_P,
                                                  InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            return AddMethod(weavingInfo_P, interceptorParameterPossibilitiesP, "method");
        }

        public static CommonWeavingInfo AddConstructor(this CommonWeavingInfo weavingInfo_P,
                                                       InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            return AddConstructor(weavingInfo_P, interceptorParameterPossibilitiesP, "constructor");
        }
    }
}