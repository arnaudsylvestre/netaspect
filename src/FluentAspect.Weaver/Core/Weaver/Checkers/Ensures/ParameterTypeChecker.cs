using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public class ParameterReferencedChecker : IChecker
    {

        public enum ReferenceModel
        {
            None,
            Referenced,
            Out,
        }

        private ReferenceModel Referenced { get; set; }

        public ParameterReferencedChecker(ReferenceModel referenced)
        {
            Referenced = referenced;
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
                errorHandler.OnError(ErrorCode.ImpossibleToOutTheParameter, FileLocation.None,
                                     parameterInfo.Name, parameterInfo.Member.Name,
                                     parameterInfo.Member.DeclaringType.FullName);
            }
        }

        public void Check(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            CheckReferenced(parameterInfo, errorHandler);
            CheckOut(parameterInfo, errorHandler);
        }
    }

    public class ParameterTypeChecker : IChecker
    {
        public ParameterTypeChecker(string expectedType, ParameterDefinition parameterDefinition)
        {
            ExpectedType = expectedType;
            this.parameterDefinition = parameterDefinition;
        }

        private string ExpectedType { get; set; }
        private readonly ParameterDefinition parameterDefinition;

        public void Check(ParameterInfo parameter, ErrorHandler errorHandler)
        {
            CheckGenericType(parameter, errorHandler);
            CheckType(parameter, errorHandler);
        }

        public void CheckType(ParameterInfo parameterInfo, IErrorListener errorHandler)
        {
            var parameterType = parameterInfo.ParameterType;
            if (parameterType == typeof (object))
                return;
            if (parameterType.FullName.Replace("&", "") != ExpectedType.Replace("/", "+").Replace("&", ""))
            {
                errorHandler.OnError(ErrorCode.ParameterWithBadType, FileLocation.None,
                   parameterInfo.Name, parameterInfo.Member.Name, parameterInfo.Member.DeclaringType.FullName.Replace("/", "+"),
                    parameterInfo.ParameterType.FullName,
                    parameterDefinition.ParameterType.FullName.Replace("/", "+"), ((IMemberDefinition)parameterDefinition.Method).Name,
                    ((IMemberDefinition)parameterDefinition.Method).DeclaringType.FullName.Replace("/", "+"));
                
            }

        }

        public void CheckGenericType(ParameterInfo info, IErrorListener errorHandler)
        {
            if (parameterDefinition == null)
                return;
            if (parameterDefinition.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
            {
                errorHandler.OnError(ErrorCode.ImpossibleToRefGenericParameter, FileLocation.None);
            }
        }
    }
}