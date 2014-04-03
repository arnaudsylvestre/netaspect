using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Errors
{
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
}