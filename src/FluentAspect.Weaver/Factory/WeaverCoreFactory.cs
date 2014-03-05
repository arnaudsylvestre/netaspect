using FluentAspect.Weaver.Core.V2;

namespace FluentAspect.Weaver.Factory
{
    public static class WeaverCoreFactory
    {
        public static WeaverCore2 CreateV2()
        {
            return new WeaverCore2(new WeavingModelComputer());
        }
    }
}