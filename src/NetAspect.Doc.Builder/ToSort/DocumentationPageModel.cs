using System.Collections.Generic;
using System.Linq;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class DocumentationPageModel
    {
        private readonly Dictionary<string, string> parameterDescriptions = new Dictionary<string, string>();
        public List<InterceptorDocumentation> Interceptors { get; set; }

        public IEnumerable<string> Members
        {
            get { return Interceptors.Select(i => i.Member).Distinct(); }
        }

        public List<ParameterModel> Parameters { get; set; }
        public WeavingModelDoc Weaving { get; set; }

        public IEnumerable<InterceptorDocumentation> GetInterceptors(string member)
        {
            return from i in Interceptors where i.Member == member select i;
        }

        public void SetParameterDescription(string parameterName, string description)
        {
            parameterDescriptions.Add(parameterName, description);
        }

        public string GetParameterDescription(string parameterName)
        {
            return parameterDescriptions[parameterName];
        }
    }
}