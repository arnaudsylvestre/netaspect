using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Errors
{
    public class ErrorHandler
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
    }
}