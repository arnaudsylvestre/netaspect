using System.Collections.Generic;
using System.Linq;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorsSectionModel
    {
        private readonly List<CsTestFile> testFiles;
        private Dictionary<string, string> parameterDescriptions;

        public InterceptorsSectionModel(List<CsTestFile> testFiles, Dictionary<string, string> parameterDescriptions)
        {
            this.testFiles = testFiles;
            this.parameterDescriptions = parameterDescriptions;
        }


        public IEnumerable<string> Members
        {
            get { return testFiles.Select(i => i.Member).Distinct(); }
        }

        public IEnumerable<CsTestFile> GetInterceptors(string member)
        {
            return from i in testFiles where i.Member == member select i;
        }

        public string GetParameterId(string parameterName)
        {
            return parameterDescriptions.GetParameterId(parameterName);
        }
    }
}