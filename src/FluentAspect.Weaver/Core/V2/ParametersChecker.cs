using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
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

    public class ParametersChecker
    {
        private List<InterceptorParametersChecker> possibleParameters = new List<InterceptorParametersChecker>();

        public void Add(InterceptorParametersChecker checker)
        {
            possibleParameters.Add(checker);
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {

            var duplicates = possibleParameters.GroupBy(s => s.ParameterName).SelectMany(grp => grp.Skip(1));
            foreach (var duplicate in duplicates)
            {
                errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", duplicate.ParameterName));
            }
            foreach (var parameterInfo in parameters)
            {
               var key_L = parameterInfo.Name.ToLower();
                try
                {
                    possibleParameters.Find(p => p.ParameterName == key_L).Checker.Check(parameterInfo, errorHandler);
                }
                catch (Exception)
                {
                  errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
               }
            }
        }

        public void AddRange(IEnumerable<InterceptorParametersChecker> s)
        {
            foreach (var interceptorParametersChecker in s)
            {
                Add(interceptorParametersChecker);
            }
        }
    }
}