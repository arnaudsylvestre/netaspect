using System;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters
{
    public static class CurrentInstancePossibility
    {
        public static CommonWeavingInfo AddCurrentParameters<T>(this CommonWeavingInfo weavingInfo_P,
                                                                InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
                                                                string parameterName) where T : VariablesForMethod
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
                                              .WhichCanNotBeReferenced()
                                              .WhichMustBeOfType<T, object[]>()
                                              .AndInjectTheVariable(variables => variables.Parameters.Definition);
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddParameters(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            return AddCurrentParameters(weavingInfo_P, interceptorParameterPossibilitiesP, "parameters");
        }

        public static CommonWeavingInfo AddInstance(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCurrentInstance(interceptorParameterPossibilitiesP, "instance");
        }


        public static CommonWeavingInfo AddParameterNames<T>(this CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP) where T : VariablesForMethod
        {
            return AddParameterNames(weavingInfo_P, interceptorParameterPossibilitiesP, () => "{0}");
        }

        public static CommonWeavingInfo AddParameterNames<T>(CommonWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
         Func<string> parameterNameFormatProvider)
          where T : VariablesForMethod
        {
            foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
            {
                interceptorParameterPossibilitiesP.AddPossibleParameter(
                   String.Format(
                      parameterNameFormatProvider(),
                      parameter.Name.ToLower()))
                   .WhichCanNotBeOut()
                   .WhichMustBeOfTypeOfParameter(parameter)
                   .AndInjectTheParameter(parameter);
            }
            return weavingInfo_P;
        }

        public static CommonWeavingInfo AddCurrentInstance<T>(this CommonWeavingInfo weavingInfo_P,
                                                     InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
                                                     string parameterName) where T : VariablesForMethod
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
                                              .WhichCanNotBeReferenced()
                                              .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
                                              .OrOfCurrentMethodDeclaringType(weavingInfo_P)
                                              .AndInjectTheCurrentInstance();
            return weavingInfo_P;
        }
    }
}