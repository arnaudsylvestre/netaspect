using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class AvailableParametersSectionModel
    {
        private readonly Dictionary<string, string> parameterDescriptions = new Dictionary<string, string>();

        public AvailableParametersSectionModel(List<ParameterModel> parameters)
        {
            Parameters = parameters;
        }


        public List<ParameterModel> Parameters { get; set; }



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