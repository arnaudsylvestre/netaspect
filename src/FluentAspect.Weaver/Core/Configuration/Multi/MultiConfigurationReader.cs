using System;
using System.Collections.Generic;
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

        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration,
                                      ErrorHandler errorHandler)
        {
            foreach (IConfigurationReader weaverEngine in engines)
            {
                weaverEngine.ReadConfiguration(types, configuration, errorHandler);
            }
        }
    }
}