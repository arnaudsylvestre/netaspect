using System.Collections.Generic;
using System.Linq;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorsSectionModel
    {
        private readonly List<CsTestFile> testFiles;

        public InterceptorsSectionModel(List<CsTestFile> testFiles)
        {
            this.testFiles = testFiles;
        }


        public IEnumerable<string> Members
        {
            get { return testFiles.Select(i => i.Member).Distinct(); }
        }

        public IEnumerable<CsTestFile> GetInterceptors(string member)
        {
            return from i in testFiles where i.Member == member select i;
        }
    }
}