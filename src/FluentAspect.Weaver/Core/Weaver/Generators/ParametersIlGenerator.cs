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
        private readonly List<ParameterConfiguration<T>> possibleParameters = new List<ParameterConfiguration<T>>();

        

        public ParameterConfiguration<T> Add(string name)
        {
            var parameterConfiguration = new ParameterConfiguration<T>(name);
            possibleParameters.Add(parameterConfiguration);
            return parameterConfiguration;
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            IEnumerable<ParameterConfiguration<T>> duplicates =
                possibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
            foreach (ParameterConfiguration<T> duplicate in duplicates)
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
    }

    public class ParameterConfiguration<T>
    {
        public class MyInterceptorParameterChecker : IInterceptorParameterChecker
        {
            public List<Action<ParameterInfo, ErrorHandler>> Checkers = new List<Action<ParameterInfo, ErrorHandler>>();

            public void Check(ParameterInfo parameter, ErrorHandler errorListener)
            {
                foreach (var checker in Checkers)
                {
                    checker(parameter, errorListener);
                }
            }
        }


        public class MyGenerator : IInterceptorParameterIlGenerator<T>
        {
            public List<Action<ParameterInfo, List<Instruction>, T>> Generators = new List<Action<ParameterInfo, List<Instruction>, T>>();

            public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
            {
                foreach (var generator in Generators)
                {
                    generator(parameterInfo, instructions, info);
                }
            }
        }

        public ParameterConfiguration(string name)
        {
            Name = name;
            Generator = new MyGenerator();
            Checker = new MyInterceptorParameterChecker();
        }

        public MyGenerator Generator { get; private set; }
        public MyInterceptorParameterChecker Checker { get; private set; }
        public string Name;
    }
}