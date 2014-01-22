using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAssembly
    {
        private List<INetAspectType> types = new List<INetAspectType>();
        private string name = "Temp";


        public NetAspectAssembly(ModuleDefinition module)
        {
            Module = module;
        }

        public ModuleDefinition Module { get; private set; }

        public void Add(NetAspectClass @class)
        {
            types.Add(@class);
        }

        public void Generate()
        {
            foreach (var type in types)
            {
                Module.Types.Add(type.TypeDefinition);
            }
        }
    }
}