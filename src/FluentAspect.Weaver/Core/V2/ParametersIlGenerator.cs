using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersIlGenerator<T>
    {
        private readonly List<Item> possibleParameters = new List<Item>();

        public void Add(string name, IInterceptorParameterIlGenerator<T> generator)
        {
            possibleParameters.Add(new Item
                {
                    Generator = generator,
                    Name = name,
                });
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
            public string Name;
        }
    }
}