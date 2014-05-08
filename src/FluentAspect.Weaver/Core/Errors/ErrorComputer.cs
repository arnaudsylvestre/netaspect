using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Errors
{
    public class ErrorInfo
    {
        public ErrorInfo(ErrorLevel level, string message)
        {
            Level = level;
            Message = message;
        }

        public ErrorLevel Level { get; set; }
        public string Message { get; set; }
    }

    public enum ErrorLevel
    {
        Warning,
        Error,
        Failure,
    }

    public class ErrorInfoComputer
    {
        private Dictionary<ErrorCode, ErrorInfo> information;

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