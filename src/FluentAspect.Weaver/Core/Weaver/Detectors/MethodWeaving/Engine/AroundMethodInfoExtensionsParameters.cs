using System.IO;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class AroundMethodInfoExtensionsParameters
    {
        public static AroundMethodInfo AddInstance(this AroundMethodInfo info)
        {
            info.AddPossibleParameter("instance")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType()
                .AndInjectTheCurrentInstance();
            return info;
        }
        public static AroundMethodInfo AddCurrentMethod(this AroundMethodInfo info)
        {
            info.AddPossibleParameter("method")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return info;
        }
        public static AroundMethodInfo AddCallerParameters(this AroundMethodInfo info)
        {
            info.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return info;
        }
        //public static AroundInstructionInfo AddCallerParameters(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("callerparameters")
        //        .WhichCanNotBeReferenced()
        //        .WhichMustBeOfType<object[]>()
        //        .AndInjectTheVariable(variables => variables.Parameters);
        //    return info;
        //}
        //public static AroundInstructionInfo AddCalledParameters(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("calledparameters")
        //        .WhichCanNotBeReferenced()
        //        .WhichMustBeOfType<object[]>()
        //        .AndInjectTheVariable(variables => variables.CalledParametersObject);
        //    return info;

        //}
        //public static AroundInstructionInfo AddCalledParameterNames(this AroundInstructionInfo info)
        //{
        //    foreach (ParameterDefinition parameterDefinition in info.GetOperandAsMethod().Parameters)
        //    {
        //        info.AddPossibleParameter("calledparameters")
        //            .WhichCanNotBeReferenced()
        //            .WhichMustBeOfTypeOfParameter(parameterDefinition)
        //            .AndInjectTheCalledParameter(parameterDefinition);
        //    }
        //    return info;

        //}
        //public static AroundInstructionInfo AddCallerParameterNames(this AroundInstructionInfo info)
        //{
        //    foreach (ParameterDefinition parameter in info.Method.Parameters)
        //    {

        //        info.AddPossibleParameter("caller" + parameter.Name.ToLower())
        //            .WhichCanNotBeOut()
        //            .WhichMustBeOfTypeOfParameter(parameter)
        //            .AndInjectTheParameter(parameter);
        //    }
        //    return info;
        //}
        //public static AroundInstructionInfo AddColumnNumber(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("columnnumber")
        //        .WhichCanNotBeReferenced()
        //        .WhichPdbPresent()
        //        .WhichMustBeOfType<int>()
        //        .AndInjectThePdbInfo(s => s.StartColumn);
        //    return info;
        //}
        //public static AroundInstructionInfo AddLineNumber(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("linenumber")
        //        .WhichCanNotBeReferenced()
        //        .WhichPdbPresent()
        //        .WhichMustBeOfType<int>()
        //        .AndInjectThePdbInfo(s => s.StartLine);
        //    return info;
        //}
        //public static AroundInstructionInfo AddFilePath(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("filepath")
        //        .WhichCanNotBeReferenced()
        //        .WhichPdbPresent()
        //        .WhichMustBeOfType<string>()
        //        .AndInjectThePdbInfo(s => s.Document.Url);
        //    return info;
        //}
        //public static AroundInstructionInfo AddFileName(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("filename")
        //        .WhichCanNotBeReferenced()
        //        .WhichPdbPresent()
        //        .WhichMustBeOfType<string>()
        //        .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url));
        //    return info;
        //}
        //public static AroundInstructionInfo AddCalledFieldInfo(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("field")
        //        .WhichCanNotBeReferenced()
        //        .WhichMustBeOfType<FieldInfo>()
        //        .AndInjectTheCalledFieldInfo();
        //    return info;
        //}
        //public static AroundInstructionInfo AddCalledPropertyInfo(this AroundInstructionInfo info)
        //{
        //    info.AddPossibleParameter("property")
        //        .WhichCanNotBeReferenced()
        //        .WhichMustBeOfType<PropertyInfo>()
        //        .AndInjectTheCalledPropertyInfo();
        //    return info;
        //}
    }
}