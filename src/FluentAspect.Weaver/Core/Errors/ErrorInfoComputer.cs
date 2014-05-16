using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Errors
{
    public class ErrorInfoComputer
    {
        private readonly Dictionary<ErrorCode, ErrorInfo> information;

        public ErrorInfoComputer(Dictionary<ErrorCode, ErrorInfo> information)
        {
            this.information = information;
        }

        public ErrorLevel ComputeLevel(ErrorCode errorCode)
        {
            return information[errorCode].Level;
        }

        public string ComputeMessage(ErrorCode errorCode, object[] parameters)
        {
            return string.Format(information[errorCode].Message, parameters);
        }
    }
}