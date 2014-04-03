using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Factory
{
    public static class WeaverCoreFactory
    {
        public static WeaverCore Create()
        {
            return new WeaverCore(new WeavingModelComputer());
        }
    }
}