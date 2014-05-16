using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver
{
    internal class MethodCompliance
    {
        public static bool IsMethod(NetAspectDefinition aspect, MethodDefinition method)
        {
            return !method.IsConstructor && !IsPropertySetterMethod(aspect, method) && !IsPropertyGetterMethod(aspect, method);
        }
        public static bool IsMethodParameter(NetAspectDefinition aspect, ParameterDefinition parameter, MethodDefinition method)
        {
            return !method.IsConstructor && !IsPropertySetterMethod(aspect, method) && !IsPropertyGetterMethod(aspect, method);
        }
        public static bool IsPropertySetterMethod(NetAspectDefinition aspect, MethodDefinition method)
        {
            return method.GetPropertyForSetter() != null;
        }
        public static bool IsPropertyGetterMethod(NetAspectDefinition aspect, MethodDefinition method)
        {
            return method.GetPropertyForGetter() != null;
        }

        public static bool IsConstructor(NetAspectDefinition aspect, MethodDefinition method)
        {
            return method.IsConstructor;
        }
    }
}