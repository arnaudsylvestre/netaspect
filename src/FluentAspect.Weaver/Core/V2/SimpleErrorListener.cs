using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class SimpleErrorListener : IErrorListener
    {
        private List<string> errors = new List<string>();
        private List<string> warnings = new List<string>(); 

        public void OnError(string message, params object[] args)
        {
            errors.Add(string.Format(message, args));
        }

        public void OnWarning(string message, params object[] args)
        {
            warnings.Add(string.Format(message, args));
        }

        public bool HasError { get { return errors.Count != 0; }}
    }
}