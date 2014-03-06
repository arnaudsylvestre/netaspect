using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersEngine
    {

        private class Parameter
        {
            public Action<ParameterInfo, ErrorHandler> Checker;

            public Action<ParameterInfo, List<Instruction>> InstructionFiller;
        }

        readonly Dictionary<string, Parameter> possibleParameters = new Dictionary<string, Parameter>();

        public void AddPossibleParameter(string linenumber, Action<ParameterInfo, ErrorHandler> check, Action<ParameterInfo, List<Instruction>> instructionFiller)
        {
            possibleParameters.Add(linenumber, new Parameter()
                {
                    Checker = check,
                    InstructionFiller = instructionFiller,
                });
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            foreach (var parameterInfo in parameters)
            {
               var key_L = parameterInfo.Name.ToLower();
               if (possibleParameters.ContainsKey(key_L))
               {
                  possibleParameters[key_L].Checker(parameterInfo, errorHandler);
               }
               else
               {
                  errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
               }
            }
        }

        public void Fill(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions)
        {
            foreach (ParameterInfo parameterInfo_L in parameters)
            {
                possibleParameters[parameterInfo_L.Name.ToLower()].InstructionFiller(parameterInfo_L, instructions);
            }
        }
    }
}