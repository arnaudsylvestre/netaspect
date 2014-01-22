using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAssembly
    {
        private List<INetAspectType> types = new List<INetAspectType>();
        private string name = "Temp";


        public NetAspectAssembly(AssemblyDefinition assemblyDefinition)
        {
            Assembly = assemblyDefinition;
        }

        public AssemblyDefinition Assembly { get; private set; }

        public void Add(NetAspectClass @class)
        {
            types.Add(@class);
        }

        public void Generate(string filename)
        {
            foreach (var type in types)
            {
                Assembly.MainModule.Types.Add(type.TypeDefinition);
            }
            Assembly.Write(filename);
        }
    }
}