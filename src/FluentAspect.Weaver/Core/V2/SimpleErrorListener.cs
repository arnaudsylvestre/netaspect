using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class SimpleErrorListener : IErrorListener
    {
        private readonly List<string> errors = new List<string>();
        private readonly List<string> warnings = new List<string>();

        public bool HasError
        {
            get { return errors.Count != 0; }
        }

        public void OnError(string message, params object[] args)
        {
            errors.Add(string.Format(message, args));
        }

        public void OnWarning(string message, params object[] args)
        {
            warnings.Add(string.Format(message, args));
        }
    }
}