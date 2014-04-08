using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Errors
{
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

        public void OnFailure(string message, params object[] args)
        {
            Failures.Add(string.Format(message, args));
        }
    }
}