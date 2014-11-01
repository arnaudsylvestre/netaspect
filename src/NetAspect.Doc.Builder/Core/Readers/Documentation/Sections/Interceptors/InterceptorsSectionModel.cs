using System.Collections.Generic;
using System.Linq;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorsSectionModel
    {
        private readonly List<CsTestFile> testFiles;
        private readonly List<ParameterDescriptionFactory.ParameterDescription> parameterDescriptions;

        public InterceptorsSectionModel(List<CsTestFile> testFiles, List<ParameterDescriptionFactory.ParameterDescription> parameterDescriptions)
        {
            this.testFiles = testFiles;
            this.parameterDescriptions = parameterDescriptions;
        }


        public List<ParameterDescriptionFactory.ParameterDescription> GetParameters(CsTestFile testFile)
        {
            var compliant = new List<ParameterDescriptionFactory.ParameterDescription>();
            foreach (var parameter in testFile.Parameters)
            {
                var parameterDescription = parameterDescriptions.FirstOrDefault(p => p.Name == GetRealParameterName(parameter) && p.InInstruction == (InterceptorModelHelper.ExtractKind(testFile.Name) == Kind.Call));
                if (parameterDescription == null)
                    parameterDescription = parameterDescriptions.FirstOrDefault(p => p.Name == GetRealParameterName(parameter) && p.InInstruction == (InterceptorModelHelper.ExtractKind(testFile.Name) == Kind.Method || InterceptorModelHelper.ExtractKind(testFile.Name) == Kind.Parameter));
                compliant.Add(parameterDescription);
            }
            return compliant;
        }


        public string GetRealParameterName(string parameterName)
        {
            return parameterDescriptions.GetRealParameterName(parameterName);
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