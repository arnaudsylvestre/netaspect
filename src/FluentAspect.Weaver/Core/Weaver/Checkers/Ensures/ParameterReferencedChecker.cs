using System.Reflection;
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
}