using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Configuration
{
    public interface IConfigurationReader
    {
       void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration, ErrorHandler errorHandler);
    }
}