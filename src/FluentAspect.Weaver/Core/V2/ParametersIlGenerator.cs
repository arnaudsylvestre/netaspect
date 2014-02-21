using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersIlGenerator
    {
        private Dictionary<string, IInterceptorParameterIlGenerator> possibleParameters = new Dictionary<string, IInterceptorParameterIlGenerator>();

        public void Add(string name, IInterceptorParameterIlGenerator checker)
        {
            possibleParameters.Add(name, checker);
        }

        public void Generate(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            foreach (var parameterInfo in parameters)
            {
                var key_L = parameterInfo.Name.ToLower();
                possibleParameters[key_L].GenerateIl(parameterInfo, instructions, info);
            }
        }
    }
}