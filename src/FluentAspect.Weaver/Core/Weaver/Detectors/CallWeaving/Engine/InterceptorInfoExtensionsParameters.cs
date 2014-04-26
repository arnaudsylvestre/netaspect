using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class InterceptorInfoExtensionsParameters
    {
        public static InterceptorInfo AddCalled(this InterceptorInfo info)
        {
            info.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType()
                .AndInjectTheCalledInstance();
            return info;
        }
        public static InterceptorInfo AddCaller(this InterceptorInfo info)
        {
            info.AddPossibleParameter("caller")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType()
                .AndInjectTheCurrentInstance();
            return info;
        }
        public static InterceptorInfo AddCallerMethod(this InterceptorInfo info)
        {
            info.AddPossibleParameter("callermethod")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<MethodBase>()
                .AndInjectTheCurrentMethod();
            return info;
        }
        public static InterceptorInfo AddCallerParameters(this InterceptorInfo info)
        {
            info.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return info;
        }
        public static InterceptorInfo AddCalledParameters(this InterceptorInfo info)
        {
            info.AddPossibleParameter("calledparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.CalledParametersObject);
            return info;

        }
        public static InterceptorInfo AddCalledParameterNames(this InterceptorInfo info)
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
        public static InterceptorInfo AddCallerParameterNames(this InterceptorInfo info)
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
        public static InterceptorInfo AddColumnNumber(this InterceptorInfo info)
        {
            info.AddPossibleParameter("columnnumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartColumn);
            return info;
        }
        public static InterceptorInfo AddLineNumber(this InterceptorInfo info)
        {
            info.AddPossibleParameter("linenumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartLine);
            return info;
        }
        public static InterceptorInfo AddFilePath(this InterceptorInfo info)
        {
            info.AddPossibleParameter("filepath")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => s.Document.Url);
            return info;
        }
        public static InterceptorInfo AddFileName(this InterceptorInfo info)
        {
            info.AddPossibleParameter("filename")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url));
            return info;
        }
        public static InterceptorInfo AddCalledFieldInfo(this InterceptorInfo info)
        {
            info.AddPossibleParameter("field")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<FieldInfo>()
                .AndInjectTheCalledFieldInfo();
            return info;
        }
        public static InterceptorInfo AddCalledPropertyInfo(this InterceptorInfo info)
        {
            info.AddPossibleParameter("property")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<PropertyInfo>()
                .AndInjectTheCalledPropertyInfo();
            return info;
        }
    }
}