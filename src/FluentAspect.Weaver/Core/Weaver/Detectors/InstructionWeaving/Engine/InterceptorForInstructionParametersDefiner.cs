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
        public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("called", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType()
                .AndInjectTheCalledInstance();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCaller(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("caller", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType()
                .AndInjectTheCurrentInstance();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCallerMethod(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("callermethod", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCallerParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("callerparameters", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("calledparameters", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.CalledParametersObject);
            return weavingInfo_P;

        }
        public static InstructionWeavingInfo AddCalledParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
            foreach (ParameterDefinition parameterDefinition in weavingInfo_P.GetOperandAsMethod().Parameters)
            {
               weavingInfo_P.AddPossibleParameter("calledparameters", interceptorParameterConfigurations_P)
                    .WhichCanNotBeReferenced()
                    .WhichMustBeOfTypeOfParameter(parameterDefinition)
                    .AndInjectTheCalledParameter(parameterDefinition);
            }
            return weavingInfo_P;

        }
        public static InstructionWeavingInfo AddCallerParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
            foreach (ParameterDefinition parameter in weavingInfo_P.MethodOfInstruction.Parameters)
            {

               weavingInfo_P.AddPossibleParameter("caller" + parameter.Name.ToLower(), interceptorParameterConfigurations_P)
                    .WhichCanNotBeOut()
                    .WhichMustBeOfTypeOfParameter(parameter)
                    .AndInjectTheParameter(parameter);
            }
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddColumnNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("columnnumber", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartColumn);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddLineNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("linenumber", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartLine);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddFilePath(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("filepath", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => s.Document.Url);
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddFileName(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("filename", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url));
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledFieldInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("field", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<FieldInfo>()
                .AndInjectTheCalledFieldInfo();
            return weavingInfo_P;
        }
        public static InstructionWeavingInfo AddCalledPropertyInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddPossibleParameter("property", interceptorParameterConfigurations_P)
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<PropertyInfo>()
                .AndInjectTheCalledPropertyInfo();
            return weavingInfo_P;
        }
    }
}