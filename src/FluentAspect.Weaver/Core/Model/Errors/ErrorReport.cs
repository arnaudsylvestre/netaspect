using System;
using System.Collections.Generic;
using System.Linq;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Model.Errors
{
    public class ErrorReport
    {
       [Serializable]
        public class Error
        {
            public ErrorLevel Level { get; set; }
            public string Message { get; set; }
        }

        public ErrorReport(List<Error> errors)
        {
            Errors = errors;
        }

        public List<Error> Errors { get; private set; }

        public IEnumerable<string> Warnings
        {
            get { return from e in Errors where e.Level == ErrorLevel.Warning select e.Message; }
        }
        public IEnumerable<string> ErrorsAndFailures
        {
            get { return from e in Errors where e.Level != ErrorLevel.Warning select e.Message; }
        }
    }
}