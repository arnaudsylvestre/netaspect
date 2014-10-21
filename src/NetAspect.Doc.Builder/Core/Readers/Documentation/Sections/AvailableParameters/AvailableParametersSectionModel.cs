﻿using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public static class ParameterDescriptionsExtensions
    {
        public static string GetRealParameterName(this Dictionary<string, string> parameterDescriptions, string parameterName)
        {
            if (parameterDescriptions.ContainsKey(parameterName))
                return parameterName;
            if (parameterName.StartsWith("caller"))
                return "caller + parameter name";
            if (parameterName.StartsWith("called"))
                return "called + parameter name";
            return "parameter name";
        }
        public static string GetParameterId(this Dictionary<string, string> parameterDescriptions, string parameterName)
        {
            return GetRealParameterName(parameterDescriptions, parameterName).Replace(" ", "").Replace("+", "");
        }
        public static string GetDescription(this Dictionary<string, string> parameterDescriptions, string parameterName)
        {
            return parameterDescriptions[parameterDescriptions.GetRealParameterName(parameterName)];
        }
    }

    public class ParameterDescriptionFactory
    {
        public static Dictionary<string, string> Create()
        {
            var parameterDescriptions = new Dictionary<string, string>();
            parameterDescriptions.Add("instance", "this parameter is used to have the <b>this</b> of the weaved member");
            parameterDescriptions.Add("method", "this parameter is used to get some information about the weaved method");
            parameterDescriptions.Add("lineNumber", "this parameter is used to get the line of the first instruction in the weaved method or the line of the weaved instruction");
            parameterDescriptions.Add("columnNumber", "this parameter is used to get the column of the first instruction in the weaved method or the column of the weaved instruction");
            parameterDescriptions.Add("fileName", "this parameter is used to get the file name of the weaved method or the file name of the weaved instruction");
            parameterDescriptions.Add("filePath", "this parameter is used to get the file path of the weaved method or the file path of the weaved instruction");
            parameterDescriptions.Add("parameters", "this parameter is used to get the values of the parameters of the weaved method");
            parameterDescriptions.Add("parameter name", "this parameter is used to get the value of the parameter with the same name of the weaved method");
            parameterDescriptions.Add("constructor", "this parameter is used to get some information about the weaved constructor");
            parameterDescriptions.Add("property", "this parameter is used to get some information about the weaved property");
            parameterDescriptions.Add("exception", "this parameter is used to get the exception thrown in the weaved method");
            parameterDescriptions.Add("result", "this parameter is used to get the return value of the weaved method");
            parameterDescriptions.Add("field", "this parameter is used to get information of the accessed field");
            parameterDescriptions.Add("newFieldValue", "this parameter is used to get the value that will be assigned to the weaved field");
            parameterDescriptions.Add("called", "this parameter is used to have the instance of the object which is used to call the weaved member");
            parameterDescriptions.Add("calledParameters", "this parameter is used to have the instance of the parameters passed to a call to a weaved method");
            parameterDescriptions.Add("called + parameter name", "this parameter is used to have the value of a parameter passed to a call to a weaved method");
            parameterDescriptions.Add("propertyValue", "this parameter is used to have the new value that will be assigned to the weaved property");
            parameterDescriptions.Add("caller", "this parameter is used to have the instance of the object that call the weaved member");
            parameterDescriptions.Add("callerParameters", "this parameter is used to have the parameters of the method that will call the weaved member");
            parameterDescriptions.Add("parameterValue", "this parameter is used to have the value of the weaved parameter");
            parameterDescriptions.Add("parameter", "this parameter is used to have the information of the weaved parameter");
            return parameterDescriptions;
        }
    }

    public class AvailableParametersSectionModel
    {
        private readonly Dictionary<string, string> parameterDescriptions;

        public AvailableParametersSectionModel(List<ParameterModel> parameters, Dictionary<string, string> parameterDescriptions)
        {
            Parameters = parameters;
            this.parameterDescriptions = parameterDescriptions;
        }


        public List<ParameterModel> Parameters { get; set; }



        public string GetRealParameterName(string parameterName)
        {
            return parameterDescriptions.GetRealParameterName(parameterName);
        }

        public string GetParameterId(string parameterName)
        {
            return parameterDescriptions.GetParameterId(parameterName);
        }
        

        public string GetParameterDescription(string parameterName)
        {
            return parameterDescriptions.GetDescription(parameterName);
        }
    }
}