using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Core.Weaver.Engine;

namespace FluentAspect.Weaver.Factory
{
    public static class WeaverCoreFactory
    {
        public static WeaverCore Create()
        {
            return new WeaverCore(new WeavingModelComputer());
        }
    }
}