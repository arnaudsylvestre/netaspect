using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public class ParametersIlGenerator<T>
    {
        private readonly List<Item> possibleParameters = new List<Item>();

        public void Add(string name, IInterceptorParameterIlGenerator<T> generator, IInterceptorParameterChecker checker)
        {
            possibleParameters.Add(new Item
                {
                    Generator = generator,
                    Name = name,
                    Checker                    = checker,
                });
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            IEnumerable<Item> duplicates =
                possibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
            foreach (Item duplicate in duplicates)
            {
                errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", duplicate.Name));
            }
            foreach (ParameterInfo parameterInfo in parameters)
            {
                string key_L = parameterInfo.Name.ToLower();
                try
                {
                    possibleParameters.Find(p => p.Name == key_L).Checker.Check(parameterInfo, errorHandler);
                }
                catch (Exception)
                {
                    errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
                }
            }
        }


        public void Generate(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, T info)
        {
            foreach (ParameterInfo parameterInfo in parameters)
            {
                string key_L = parameterInfo.Name.ToLower();
                possibleParameters.Find(i => i.Name == key_L).Generator.GenerateIl(parameterInfo, instructions, info);
            }
        }

        private class Item
        {
            public IInterceptorParameterIlGenerator<T> Generator;
            public IInterceptorParameterChecker Checker;
            public string Name;
        }
    }
}