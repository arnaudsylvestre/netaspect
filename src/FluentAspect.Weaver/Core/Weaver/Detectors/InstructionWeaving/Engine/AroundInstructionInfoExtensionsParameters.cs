using System.IO;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Helpers;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{
    public static class AroundInstructionInfoExtensionsParameters
    {
        public static AroundInstructionInfo AddCalled(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType()
                .AndInjectTheCalledInstance();
            return info;
        }
        public static AroundInstructionInfo AddCaller(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("caller")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType()
                .AndInjectTheCurrentInstance();
            return info;
        }
        public static AroundInstructionInfo AddCallerMethod(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("callermethod")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return info;
        }
        public static AroundInstructionInfo AddCallerParameters(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return info;
        }
        public static AroundInstructionInfo AddCalledParameters(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("calledparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.CalledParametersObject);
            return info;

        }
        public static AroundInstructionInfo AddCalledParameterNames(this AroundInstructionInfo info)
        {
            foreach (ParameterDefinition parameterDefinition in info.GetOperandAsMethod().Parameters)
            {
                info.AddPossibleParameter("calledparameters")
                    .WhichCanNotBeReferenced()
                    .WhichMustBeOfTypeOfParameter(parameterDefinition)
                    .AndInjectTheCalledParameter(parameterDefinition);
            }
            return info;

        }
        public static AroundInstructionInfo AddCallerParameterNames(this AroundInstructionInfo info)
        {
            foreach (ParameterDefinition parameter in info.Method.Parameters)
            {

                info.AddPossibleParameter("caller" + parameter.Name.ToLower())
                    .WhichCanNotBeOut()
                    .WhichMustBeOfTypeOfParameter(parameter)
                    .AndInjectTheParameter(parameter);
            }
            return info;
        }
        public static AroundInstructionInfo AddColumnNumber(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("columnnumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartColumn);
            return info;
        }
        public static AroundInstructionInfo AddLineNumber(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("linenumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartLine);
            return info;
        }
        public static AroundInstructionInfo AddFilePath(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("filepath")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => s.Document.Url);
            return info;
        }
        public static AroundInstructionInfo AddFileName(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("filename")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url));
            return info;
        }
        public static AroundInstructionInfo AddCalledFieldInfo(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("field")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<FieldInfo>()
                .AndInjectTheCalledFieldInfo();
            return info;
        }
        public static AroundInstructionInfo AddCalledPropertyInfo(this AroundInstructionInfo info)
        {
            info.AddPossibleParameter("property")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<PropertyInfo>()
                .AndInjectTheCalledPropertyInfo();
            return info;
        }
    }
}