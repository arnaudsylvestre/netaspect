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

        public string GetRealParameterName(string parameterName)
        {
            if (parameterDescriptions.ContainsKey(parameterName))
                return parameterName;
            if (parameterName.StartsWith("caller"))
                return "caller + parameter name";
            if (parameterName.StartsWith("called"))
                return "called + parameter name";
            return "parameter name";
        }

        public string GetParameterDescription(string parameterName)
        {


            return parameterDescriptions[GetRealParameterName(parameterName)];
        }
    }
}