using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Methods.Interceptors
{
    public class Variables
    {

        public const string ParameterParameters = "parameters";
        public const string Instance = "instance";
        public const string Method = "method";

        public List<VariableDefinition> Interceptors;
        public VariableDefinition methodInfo;
        public VariableDefinition args;
        public VariableDefinition handleResult;
    }
}