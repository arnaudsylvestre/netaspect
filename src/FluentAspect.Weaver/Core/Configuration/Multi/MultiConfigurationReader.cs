using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Configuration.Multi
{
    public class MultiConfigurationReader : IConfigurationReader
    {
        private readonly IEnumerable<IConfigurationReader> engines;

        public MultiConfigurationReader(params IConfigurationReader[] engines_P)
        {
            engines = engines_P;
        }

        public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (var weaverEngine in engines)
            {
                weaverEngine.ReadConfiguration(assembly, configuration, errorHandler);
            }
        }
    }
}