using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Helpers;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{
    public static class InterceptorParametersDefinerForInstruction
    {
        public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic(weavingInfo_P)
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType(weavingInfo_P)
                .AndInjectTheCalledInstance();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCaller(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("caller")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType(weavingInfo_P)
                .AndInjectTheCurrentInstance();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCallerMethod(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("callermethod")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCallerParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("calledparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.CalledParametersObject);
            return weavingInfo_P;

        }
        public static InstructionWeavingInfo AddCalledParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            foreach (ParameterDefinition parameterDefinition in weavingInfo_P.GetOperandAsMethod().Parameters)
            {
               interceptorParameterConfigurations_P.AddPossibleParameter("calledparameters")
                    .WhichCanNotBeReferenced()
                    .WhichMustBeOfTypeOfParameter(parameterDefinition)
                    .AndInjectTheCalledParameter(parameterDefinition);
            }
            return weavingInfo_P;

        }
        public static InstructionWeavingInfo AddCallerParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
            {

               interceptorParameterConfigurations_P.AddPossibleParameter("caller" + parameter.Name.ToLower())
                    .WhichCanNotBeOut()
                    .WhichMustBeOfTypeOfParameter(parameter)
                    .AndInjectTheParameter(parameter);
            }
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddColumnNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("columnnumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent(weavingInfo_P)
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartColumn, weavingInfo_P);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddLineNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("linenumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent(weavingInfo_P)
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartLine, weavingInfo_P);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddFilePath(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("filepath")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent(weavingInfo_P)
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => s.Document.Url, weavingInfo_P);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddFileName(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("filename")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent(weavingInfo_P)
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledFieldInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("field")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<FieldInfo>()
                .AndInjectTheCalledFieldInfo(weavingInfo_P);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledPropertyInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("property")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<PropertyInfo>()
                .AndInjectTheCalledPropertyInfo(weavingInfo_P);
            return weavingInfo_P;
        }
    }
}