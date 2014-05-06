using System.Reflection;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine
{
    public static class InterceptorParametersDefinerForMethod
    {
        public static MethodWeavingInfo AddInstance(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("instance")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType(weavingInfo_P)
                .AndInjectTheCurrentInstance();
            return weavingInfo_P;
        }
        public static MethodWeavingInfo AddCurrentMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("method")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return weavingInfo_P;
        }
        public static MethodWeavingInfo AddCallerParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           interceptorParameterConfigurations_P.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return weavingInfo_P;
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