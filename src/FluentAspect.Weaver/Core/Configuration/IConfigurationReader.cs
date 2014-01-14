using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Configuration
{
    public interface IConfigurationReader
    {
       void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler);
    }
}