using System.Reflection;

namespace FluentAspect.Weaver.Core.Model
{
    public static class NetAspectAttributeExtensions
    {
        public static MethodInfo GetInterceptorMethod(this object netAspectAttribute, string methodName)
        {
            return netAspectAttribute.GetType().GetMethod(methodName, NetAspectAttribute._bindingFlags);
        }
    }
}