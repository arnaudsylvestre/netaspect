using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;

namespace FluentAspect.Weaver.Core
{
    public interface IWeaverBuilder
    {
        IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration);
    }
}