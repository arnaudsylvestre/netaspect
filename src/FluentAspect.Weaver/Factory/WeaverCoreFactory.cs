using FluentAspect.Weaver.Core.V2;

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