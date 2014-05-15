using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
    public static class InterceptorParametersDefinerForInstruction
    {
        public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P, IMemberDefinition member)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic(member)
                .WhichMustBeOfTypeOf(member.DeclaringType)
                .AndInjectTheCalledInstance();
            return weavingInfo_P;
        }
        public static MethodWeavingInfo AddCaller(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddCaller(weavingInfo_P, interceptorParameterConfigurations_P, "caller");
        }
        public static MethodWeavingInfo AddInstance(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddCaller(weavingInfo_P, interceptorParameterConfigurations_P, "instance");
        }

        private static MethodWeavingInfo AddCaller(MethodWeavingInfo weavingInfo_P,
                                                   InterceptorParameterConfigurations interceptorParameterConfigurations_P,
                                                   string parameterName)
        {
            interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
                                                .WhichCanNotBeReferenced()
                                                .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
                                                .OrOfCurrentMethodDeclaringType(weavingInfo_P)
                                                .AndInjectTheCurrentInstance();
            return weavingInfo_P;
        }

        public static MethodWeavingInfo AddCallerMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "callermethod");
        }

        private static MethodWeavingInfo AddMethod(MethodWeavingInfo weavingInfo_P,
                                                        InterceptorParameterConfigurations interceptorParameterConfigurations_P,
                                                        string parameterName)
        {
            interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
                                                .WhichCanNotBeReferenced()
                                                .WhichMustBeOfType<MethodBase>()
                                                .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static MethodWeavingInfo AddProperty(this MethodWeavingInfo weavingInfo_P,
                                                        InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            interceptorParameterConfigurations_P.AddPossibleParameter("property")
                                                .WhichCanNotBeReferenced()
                                                .WhichMustBeOfType<PropertyInfo>()
                                                .AndInjectTheCurrentProperty();
            return weavingInfo_P;
        }

        public static MethodWeavingInfo AddMethod(this MethodWeavingInfo weavingInfo_P,
                                                        InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "method");
        }
        public static MethodWeavingInfo AddConstructor(this MethodWeavingInfo weavingInfo_P,
                                                        InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "constructor");
        }
        public static MethodWeavingInfo AddCallerParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddCurrentParameters(weavingInfo_P, interceptorParameterConfigurations_P, "callerparameters");
        }
        public static MethodWeavingInfo AddParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddCurrentParameters(weavingInfo_P, interceptorParameterConfigurations_P, "parameters");
        }

        private static MethodWeavingInfo AddCurrentParameters(MethodWeavingInfo weavingInfo_P,
                                                                   InterceptorParameterConfigurations
                                                                       interceptorParameterConfigurations_P,
                                                                   string parameterName)
        {
            interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
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
                interceptorParameterConfigurations_P.AddPossibleParameter("called" + parameterDefinition.Name.ToLower())
                    .WhichCanNotBeReferenced()
                    .WhichMustBeOfTypeOfParameter(parameterDefinition)
                    .AndInjectTheCalledParameter(parameterDefinition);
            }
            return weavingInfo_P;

        }
        
        public static MethodWeavingInfo AddCallerParameterNames(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddParameterNames(weavingInfo_P, interceptorParameterConfigurations_P, () => "caller{0}");
        }
        public static MethodWeavingInfo AddParameterNames(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            return AddParameterNames(weavingInfo_P, interceptorParameterConfigurations_P, () => "{0}");
        }

        private static MethodWeavingInfo AddParameterNames(MethodWeavingInfo weavingInfo_P,
                                                           InterceptorParameterConfigurations
                                                               interceptorParameterConfigurations_P,
                                                           Func<string> parameterNameFormatProvider)
        {
            foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
            {
                interceptorParameterConfigurations_P.AddPossibleParameter(string.Format(parameterNameFormatProvider(),
                                                                                        parameter.Name.ToLower()))
                                                    .WhichCanNotBeOut()
                                                    .WhichMustBeOfTypeOfParameter(parameter)
                                                    .AndInjectTheParameter(parameter);
            }
            return weavingInfo_P;
        }


        public static MethodWeavingInfo AddValue(this MethodWeavingInfo weavingInfo_P,
                                                           InterceptorParameterConfigurations
                                                               interceptorParameterConfigurations_P)
        {
            var parameter = weavingInfo_P.Method.Parameters[0];
            interceptorParameterConfigurations_P.AddPossibleParameter("value")
                                                .WhichCanNotBeOut()
                                                .WhichMustBeOfTypeOfParameter(parameter)
                                                .AndInjectTheParameter(parameter);
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
        public static MethodWeavingInfo AddException(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("exception")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<Exception>()
                .AndInjectTheVariable(variables => variables.Exception);
            return weavingInfo_P;
        }
        public static MethodWeavingInfo AddResult(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("result")
                .WhereParameterTypeIsSameAsMethodResult(weavingInfo_P)
                .AndInjectTheVariable(variables => variables.Result);
            return weavingInfo_P;
        }
    }
}