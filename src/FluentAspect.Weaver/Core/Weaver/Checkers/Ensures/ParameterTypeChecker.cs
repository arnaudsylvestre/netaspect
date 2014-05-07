using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public class ParameterTypeChecker : IChecker
    {
        public ParameterTypeChecker(ReferenceModel referenced, string expectedType)
        {
            Referenced = referenced;
            ExpectedType = expectedType;
        }

        public enum ReferenceModel
        {
            None,
            Referenced,
            Out,
        }

        ReferenceModel Referenced { get; set; }
        string ExpectedType { get; set; }

        public void Check(ParameterInfo parameter, ErrorHandler errorHandler)
        {
            
        }

        public void CheckType(ParameterInfo parameterInfo, IErrorListener errorHandler)
        {
            var parameterType = parameterInfo.ParameterType;
            if (parameterType == typeof (object))
                return;
            if (parameterType.FullName.Replace("&", "") != ExpectedType.Replace("/", "+").Replace("&", ""))
            {
                errorHandler.OnError(ErrorCode.ParameterWithBadType, FileLocation.None);
                
            }

        }

        public void CheckGenericType(ParameterInfo info, IErrorListener errorHandler, ParameterDefinition parameter)
        {
            if (parameter.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
            {
                errorHandler.OnError("Impossible to ref a generic parameter");
            }
        }

        public void CheckReferenced(ParameterInfo parameterInfo, IErrorListener errorHandler)
        {
            if (Referenced != ReferenceModel.None)
                return;
            if (parameterInfo.ParameterType.IsByRef)
            {
                errorHandler.OnError(ErrorCode.ImpossibleToReferenceTheParameter, FileLocation.None,
                                     parameterInfo.Name, parameterInfo.Member.Name,
                                     parameterInfo.Member.DeclaringType.FullName);
            }
        }


        public void CheckOut(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (Referenced == ReferenceModel.Out)
                return;
            if (parameterInfo.IsOut)
            {
                errorHandler.OnError("impossible to out the parameter '{0}' in the method {1} of the type '{2}'",
                                     parameterInfo.Name, parameterInfo.Member.Name,
                                     parameterInfo.Member.DeclaringType.FullName);
            }
        }
    }
}