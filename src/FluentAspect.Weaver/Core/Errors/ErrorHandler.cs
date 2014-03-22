using System.Collections.Generic;
using FluentAspect.Weaver.Core.V2.Weaver.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;

namespace FluentAspect.Weaver.Core.Errors
{
    public enum ErrorKind
    {

    }

    public enum WarningKind
    {

    }

    public enum FailureKind
    {
        Unknown
    }

    public interface IErrorTextProvider
    {
        string GetMessage(ErrorKind kind);
    }

    public class NetAspectError
    {
        private List<object> parameters;
        private IErrorTextProvider errorTextProvider;

        public NetAspectError(List<object> parameters, ErrorKind kind, IErrorTextProvider errorTextProvider)
        {
            this.parameters = parameters;
            Kind = kind;
            this.errorTextProvider = errorTextProvider;
        }

        public ErrorKind Kind { get; set; }

        public override string ToString()
        {
            return string.Format(errorTextProvider.GetMessage(Kind), parameters.ToArray());
        }
    }

    public class ErrorHandler : IErrorListener
    {
        public ErrorHandler()
        {
            Warnings = new List<string>();
            Errors = new List<string>();
            Failures = new List<string>();
        }

        public List<string> Warnings { get; private set; }
        public List<string> Errors { get; private set; }
        public List<string> Failures { get; private set; }

        public void OnError(string message, params object[] args)
        {
            Errors.Add(string.Format(message, args));
        }

        public void OnWarning(string message, params object[] args)
        {
            Warnings.Add(string.Format(message, args));
        }
    }
}