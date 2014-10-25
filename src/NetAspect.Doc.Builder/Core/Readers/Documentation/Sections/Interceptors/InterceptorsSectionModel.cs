using System.Collections.Generic;
using System.Linq;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorsSectionModel
    {
        private readonly List<CsTestFile> testFiles;
        private readonly Dictionary<string, string> parameterDescriptions;
        private readonly AvailableParametersSectionModel availableParametersSectionModel;

        public InterceptorsSectionModel(List<CsTestFile> testFiles, Dictionary<string, string> parameterDescriptions, AvailableParametersSectionModel availableParametersSectionModel)
        {
            this.testFiles = testFiles;
            this.parameterDescriptions = parameterDescriptions;
            this.availableParametersSectionModel = availableParametersSectionModel;
        }


        public List<ParameterModel> GetParameters(CsTestFile testFile)
        { return availableParametersSectionModel.Parameters.Where(p => testFile.Parameters.Contains(p.Name)).ToList(); }


        public string GetRealParameterName(string parameterName)
        {
            return parameterDescriptions.GetRealParameterName(parameterName);
        }


        public string GetParameterDescription(string parameterName)
        {
            return parameterDescriptions.GetDescription(parameterName);
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