using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver
{
    public static class WeaverFactory
    {
        public static WeaverCore Create()
        {
            return new WeaverCore(new WeavingModelComputer());
        }
    }
}